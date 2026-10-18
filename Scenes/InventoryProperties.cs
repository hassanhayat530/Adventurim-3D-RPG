using Godot;
using System;
using System.Linq;

public partial class InventoryProperties : GridContainer
{
	[Export] public string slotsType { get; set; }
	InvUISlot[] childSlots;

	public override void _Ready()
	{
		childSlots = GetChildren()
				.Select(child => child as InvUISlot)
				.Where(slot => slot != null)
				.ToArray();

		updateProperties();
	}

	public void updateProperties()
	{
		for (int i = 0; i < childSlots.Length; ++i)
		{
			childSlots[i].SlotType = slotsType;
		}
	}
}
