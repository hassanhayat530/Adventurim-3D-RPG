using Godot;
using System;
using System.Linq;

public partial class InventoryInstance : Control
{
	[Export] public Inventory inventory;
	[Export] public string slotsType { get; set; } = "";
	private InventoryProperties inventoryProperties;
    private InvUISlot[] slots;

	public override void _Ready()
	{
		var gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
		slots = gridContainer.GetChildren()
			.Select(child => child as InvUISlot)
			.Where(slot => slot != null)
			.ToArray();

		inventory.Update2 += updateSlots;
		updateSlots();
		Visible = false;

		inventoryProperties = gridContainer as InventoryProperties;
		inventoryProperties.slotsType = slotsType;
		inventoryProperties.updateProperties();
	}

    public void updateSlots()
	{
		int minSize = Math.Min(inventory.slots.Length, slots.Length);
		for (int i = 0; i < minSize; ++i)
		{
			slots[i].update(inventory.slots[i]);
		}
	}
}