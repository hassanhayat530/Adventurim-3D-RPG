using Godot;
using System;
using System.Linq;

public partial class InventoryInstance : Control
{
	[Export] public Inventory Inventory;
	[Export] public string SlotsType { get; set; } = "";
	private InventoryProperties _inventoryProperties;
    private InvUISlot[] _slots;

	public override void _Ready()
	{
		var gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
		_slots = gridContainer.GetChildren()
			.Select(child => child as InvUISlot)
			.Where(slot => slot != null)
			.ToArray();

		Inventory.Update2 += UpdateSlots;
		UpdateSlots();
		Visible = false;

		_inventoryProperties = gridContainer as InventoryProperties;
		_inventoryProperties.slotsType = SlotsType;
		_inventoryProperties.updateProperties();
	}

    public void UpdateSlots()
	{
		int minSize = Math.Min(Inventory.Slots.Length, _slots.Length);
		for (int i = 0; i < minSize; ++i)
		{
			_slots[i].UpdateSlot(Inventory.Slots[i]);
		}
	}
}