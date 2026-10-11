using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

public partial class InvUISlot : Panel
{
	[Signal] public delegate void UpdateEventHandler();
	private Sprite2D itemVisual;
	private Label amountText;
	private Sprite2D equipSymbol;
	private ToolTipInteract button;
	private Inventory playerInventory = GlobalScript.Instance.PlayerInventory;
	private Inventory merchantInventory = GlobalScript.Instance.MerchantInventory;
	public InventorySlot inventorySlot = null;
	private InventoryItem goldReference = GD.Load<InventoryItem>("res://Inventory/Items/Gold.tres");
	private InvUISlot[] siblings;
	[Export] public string slotType { get; set; } = "";
	[Export] public int number;
	private AudioStreamPlayer2D weaponEquipSound;
	private AudioStreamPlayer2D armorEquipSound;
	private AudioStreamPlayer2D ringEquipSound;
	private AudioStreamPlayer2D denySound;
	public override void _Ready()
	{

		itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		amountText = GetNode<Label>("CenterContainer/Panel/Label");
		equipSymbol = GetNode<Sprite2D>("CenterContainer/Panel/EquipSign");
		button = GetNode<ToolTipInteract>("Button");

		weaponEquipSound = GetNode<AudioStreamPlayer2D>("WeaponEquipSound");
		armorEquipSound = GetNode<AudioStreamPlayer2D>("ArmorEquipSound");
		ringEquipSound = GetNode<AudioStreamPlayer2D>("RingEquipSound");
		denySound = GetNode<AudioStreamPlayer2D>("DenySound");

		GridContainer parent = GetParent<GridContainer>();
		siblings = parent.GetChildren()
				.Select(child => child as InvUISlot)
				.Where(slot => slot != null)
				.ToArray();

		equipSymbol.Visible = false;
	}

	public void update(InventorySlot slot)
	{
		inventorySlot = slot;
		if (slot.item == null)
		{
			itemVisual.Visible = false;
			amountText.Visible = false;
			button.Visible = false;
		}
		else
		{
			itemVisual.Visible = true;
			equipSymbol.Visible = slot.equipped;
			itemVisual.Texture = slot.item.Texture;
			amountText.Text = slot.amount.ToString();
			button.Visible = true;
			if (slot.amount > 1)
			{
				amountText.Visible = true;
			}
			else
			{
				amountText.Visible = false;
			}
		}

		EmitSignal(SignalName.Update);
	}

	/// <summary>
	/// Unequips any slot with the same item type.
	/// </summary>
	/// <param name="equipType">The inventory item to unequip</param>
	public void ResetEquipped(string equipType)
	{
		for (int i = 0; i < siblings.Length; ++i)
		{
			if (siblings[i].inventorySlot != null &&
			siblings[i].inventorySlot.item != null &&
			siblings[i].inventorySlot.item.Type == equipType)
			{
				siblings[i].equip(false);
			}
		}
	}

	private void equip(bool equipping)
	{
		equipSymbol.Visible = equipping;
		inventorySlot.equipped = equipping;
	}

	public void OnButtonPressed()
	{
		InventoryItem itemHeld = inventorySlot.item;
		if (itemHeld != null)
		{
			if (itemHeld.Type == "Head" ||
				itemHeld.Type == "Chest" ||
				itemHeld.Type == "Hands" ||
				itemHeld.Type == "Legs")
			{
				armorEquipSound.Play();
			}
			if (itemHeld.Type == "Weapon")
			{
				weaponEquipSound.Play();
			}
			if (itemHeld.Type == "Ring1" ||
				itemHeld.Type == "Ring2")
			{
				ringEquipSound.Play();
			}

			if (slotType == "Player Inventory")
			{
				playerInventoryFunctionality(itemHeld);
			}

			if (slotType == "Player Shop")
			{
				playerShopFunctionality(itemHeld);
			}

			if (slotType == "Merchant")
			{
				merchantFunctionality(itemHeld);
			}
		}
	}

	public void onMouseEntered()
	{
		GetNode<Control>("CenterContainer/Panel/Highlighter").Visible = true;
	}

	public void onMouseExited()
	{
		GetNode<Control>("CenterContainer/Panel/Highlighter").Visible = false;
	}

	public void playerInventoryFunctionality(InventoryItem itemHeld)
	{
		Control equipment = GetNode<Control>("../../../Equipment");  // Get Equipment slots parent node
		EquipmentSlot[] equipmentSlots = equipment.GetChildren()  // Get list of equipment slots
					.Select(child => child as EquipmentSlot)
					.Where(slot => slot != null)
					.ToArray();

		if (itemHeld.Type == "Armor" ||
			itemHeld.Type == "Weapon" ||
			itemHeld.Type == "Ring1" ||
			itemHeld.Type == "Ring2")  // Check if Item is Equippable
		{
			string type = itemHeld.Type == "Armor" ? itemHeld.ArmorType : itemHeld.Type;  // If the item is an armor piece,
				                                                                          // get it's type, else general type
			if (!equipSymbol.Visible)  // If Item is not already equipped
			{

				ResetEquipped(type); // Unequip any other item with same type

				GlobalScript.Instance.Equipment[type] = itemHeld;
				equip(true);

				for (int i = 0; i < equipmentSlots.Length; ++i)
				{
					EquipmentSlot currEquipmentSlot = equipmentSlots[i];
					if (currEquipmentSlot.equipmentType == type)
					{
						currEquipmentSlot.fill(GlobalScript.Instance.Equipment[currEquipmentSlot.equipmentType]);
					}
				}
			}
			else
			{
				GlobalScript.Instance.Equipment[type] = null;
				equip(false);
				for (int i = 0; i < equipmentSlots.Length; ++i)
				{
					EquipmentSlot currEquipmentSlot = equipmentSlots[i];
					if (currEquipmentSlot.equipmentType == type)
					{
						currEquipmentSlot.empty();
					}
				}
			}
		}

	}

	public void playerShopFunctionality(InventoryItem item)
	{
		if (item == goldReference)
		{
			denySound.Play();
			return;
		}

		int saleValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerSellingRatio); 

		playerInventory.Insert(goldReference, saleValue);
		playerInventory.removeAt(number - 1);

		// unequip sold item if it was equipped
		foreach(KeyValuePair<string, InventoryItem> entry in GlobalScript.Instance.Equipment)
		{
			if (entry.Value == item)
			{
				GlobalScript.Instance.Equipment[entry.Key] = null;
				inventorySlot.equipped = false;
			}
		}



		GlobalScript.Instance.PlayerGold += saleValue;
	}

	public void merchantFunctionality(InventoryItem item)
	{
		int BuyingValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerBuyingRatio);
		if (GlobalScript.Instance.PlayerGold >= BuyingValue)
		{
			merchantInventory.removeAt(number - 1);
			playerInventory.Insert(item);

			playerInventory.remove(goldReference, BuyingValue);
			GlobalScript.Instance.PlayerGold -= BuyingValue;
		}
		else
		{
			denySound.Play();
		}
	}
}
