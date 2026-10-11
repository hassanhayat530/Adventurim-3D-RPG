using Godot;
using System;
using System.ComponentModel;

public partial class EquipHand : Panel
{
	[Export] public string hand { get; set; } = "Right";
	[Export] public Hand handNode { get; set; }
	private Sprite2D itemDisplay;
	InventoryUi inventoryUi;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemDisplay = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		updateHand();
		inventoryUi = GetNode<InventoryUi>("../InventoryUI");
		inventoryUi.Update += updateHand;
	}

	public void updateHand()
	{
		// Get the item in the hand and update the display
		InventoryItem itemHeld = hand == "Right" ? GlobalScript.Instance.Equipment["Weapon"] : GlobalScript.Instance.Equipment["Ring1"];

		// If there is no item, hide the display
		if (itemHeld == null)
		{
			itemDisplay.Visible = false;
			handNode.unequip();
			return;
		}

		// If there is an item, show the display and update the texture
		else
		{
			itemDisplay.Visible = true;
			
			if (hand == "Right")
			{
				handNode.equip(itemHeld.Scene);
			}

		}

		itemDisplay.Texture = itemHeld.Texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
