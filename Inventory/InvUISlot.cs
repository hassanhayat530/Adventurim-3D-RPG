using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

public partial class InvUISlot : Panel
{
	[Signal] public delegate void UpdateEventHandler();
	private Sprite2D _itemVisual;
	private Label _amountText;
	private Sprite2D _equipSymbol;
	private ToolTipInteract _button;
	private Inventory _playerInventory = GlobalScript.Instance.PlayerInventory;
	private Inventory _merchantInventory = GlobalScript.Instance.MerchantInventory;
	public InventorySlot _inventorySlot = null;
	private InventoryItem _goldReference = GD.Load<InventoryItem>("res://Inventory/Items/Gold.tres");
	private InvUISlot[] _siblings;
	[Export] public string SlotType { get; set; } = "";
	[Export] public int Number;
	private AudioStreamPlayer2D _weaponEquipSound;
	private AudioStreamPlayer2D _armorEquipSound;
	private AudioStreamPlayer2D _ringEquipSound;
	private AudioStreamPlayer2D _denySound;
	public override void _Ready()
	{

		_itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		_amountText = GetNode<Label>("CenterContainer/Panel/Label");
		_equipSymbol = GetNode<Sprite2D>("CenterContainer/Panel/EquipSign");
		_button = GetNode<ToolTipInteract>("Button");

		_weaponEquipSound = GetNode<AudioStreamPlayer2D>("WeaponEquipSound");
		_armorEquipSound = GetNode<AudioStreamPlayer2D>("ArmorEquipSound");
		_ringEquipSound = GetNode<AudioStreamPlayer2D>("RingEquipSound");
		_denySound = GetNode<AudioStreamPlayer2D>("DenySound");

		GridContainer parent = GetParent<GridContainer>();
		_siblings = parent.GetChildren()
				.Select(child => child as InvUISlot)
				.Where(slot => slot != null)
				.ToArray();

		_equipSymbol.Visible = false;
	}

	public void UpdateSlot(InventorySlot slot)
	{
		_inventorySlot = slot;
		if (slot.item == null)
		{
			_itemVisual.Visible = false;
			_amountText.Visible = false;
			_button.Visible = false;
		}
		else
		{
			_itemVisual.Visible = true;
			_equipSymbol.Visible = slot.equipped;
			_itemVisual.Texture = slot.item.Texture;
			_amountText.Text = slot.amount.ToString();
			_button.Visible = true;
			if (slot.amount > 1)
			{
				_amountText.Visible = true;
			}
			else
			{
				_amountText.Visible = false;
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
		for (int i = 0; i < _siblings.Length; ++i)
		{
			if (_siblings[i]._inventorySlot != null &&
			_siblings[i]._inventorySlot.item != null &&
			_siblings[i]._inventorySlot.item.Type == equipType)
			{
				_siblings[i].Equip(false);
			}
		}
	}

	private void Equip(bool equipping)
	{
		_equipSymbol.Visible = equipping;
		_inventorySlot.equipped = equipping;
	}

	public void OnButtonPressed()
	{
		InventoryItem itemHeld = _inventorySlot.item;
		if (itemHeld != null)
		{
			if (itemHeld.Type == "Head" ||
				itemHeld.Type == "Chest" ||
				itemHeld.Type == "Hands" ||
				itemHeld.Type == "Legs")
			{
				_armorEquipSound.Play();
			}
			if (itemHeld.Type == "Weapon")
			{
				_weaponEquipSound.Play();
			}
			if (itemHeld.Type == "Ring1" ||
				itemHeld.Type == "Ring2")
			{
				_ringEquipSound.Play();
			}

			if (SlotType == "Player Inventory")
			{
				PlayerInventoryFunctionality(itemHeld);
			}

			if (SlotType == "Player Shop")
			{
				PlayerShopFunctionality(itemHeld);
			}

			if (SlotType == "Merchant")
			{
				MerchantFunctionality(itemHeld);
			}
		}
	}

	public void OnMouseEntered()
	{
		GetNode<Control>("CenterContainer/Panel/Highlighter").Visible = true;
	}

	public void OnMouseExited()
	{
		GetNode<Control>("CenterContainer/Panel/Highlighter").Visible = false;
	}

	public void PlayerInventoryFunctionality(InventoryItem itemHeld)
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
			if (!_equipSymbol.Visible)  // If Item is not already equipped
			{

				ResetEquipped(type); // Unequip any other item with same type

				GlobalScript.Instance.Equipment[type] = itemHeld;
				Equip(true);

				for (int i = 0; i < equipmentSlots.Length; ++i)
				{
					EquipmentSlot currEquipmentSlot = equipmentSlots[i];
					if (currEquipmentSlot.EquipmentType == type)
					{
						currEquipmentSlot.Fill(GlobalScript.Instance.Equipment[currEquipmentSlot.EquipmentType]);
					}
				}
			}
			else
			{
				GlobalScript.Instance.Equipment[type] = null;
				Equip(false);
				for (int i = 0; i < equipmentSlots.Length; ++i)
				{
					EquipmentSlot currEquipmentSlot = equipmentSlots[i];
					if (currEquipmentSlot.EquipmentType == type)
					{
						currEquipmentSlot.Empty();
					}
				}
			}
		}

	}

	public void PlayerShopFunctionality(InventoryItem item)
	{
		if (item == _goldReference)
		{
			_denySound.Play();
			return;
		}

		int saleValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerSellingRatio); 

		_playerInventory.Insert(_goldReference, saleValue);
		_playerInventory.RemoveAt(GetIndex());

		// unequip sold item if it was equipped
		foreach(KeyValuePair<string, InventoryItem> entry in GlobalScript.Instance.Equipment)
		{
			if (entry.Value == item)
			{
				GlobalScript.Instance.Equipment[entry.Key] = null;
				_inventorySlot.equipped = false;
			}
		}



		GlobalScript.Instance.PlayerGold += saleValue;
	}

	public void MerchantFunctionality(InventoryItem item)
	{
		int BuyingValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerBuyingRatio);
		if (GlobalScript.Instance.PlayerGold >= BuyingValue)
		{

			_merchantInventory.RemoveAt(GetIndex());
			_playerInventory.Insert(item);

			_playerInventory.Remove(_goldReference, BuyingValue);
			GlobalScript.Instance.PlayerGold -= BuyingValue;
		}
		else
		{
			_denySound.Play();
		}
	}
}
