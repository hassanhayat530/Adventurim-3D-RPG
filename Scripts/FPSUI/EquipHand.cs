using Godot;
using System;
using System.ComponentModel;

public partial class EquipHand : Panel
{
	[Export] public string Hand { get; set; } = "Right";
	[Export] public Hand HandNode { get; set; }
	private Sprite2D _itemDisplay;
	private InventoryUi _inventoryUi;

	public override void _Ready()
	{
		_itemDisplay = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		UpdateHand();
		_inventoryUi = GetNode<InventoryUi>("../InventoryUI");
		_inventoryUi.Update += UpdateHand;
	}

	public void UpdateHand()
	{
		// Get the item in the hand and update the display
		InventoryItem itemHeld = Hand == "Right" ? GlobalScript.Instance.Equipment["Weapon"] : GlobalScript.Instance.Equipment["Ring1"];

		// If there is no item, hide the display
		if (itemHeld == null)
		{
			_itemDisplay.Visible = false;
			HandNode.unequip();
			return;
		}

		// If there is an item, show the display and update the texture
		else
		{
			_itemDisplay.Visible = true;
			
			if (Hand == "Right")
			{
				HandNode.equip(itemHeld.Scene);
			}

		}

		_itemDisplay.Texture = itemHeld.Texture;
	}
}
