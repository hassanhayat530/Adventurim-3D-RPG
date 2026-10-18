using Godot;
using System;
using System.ComponentModel;
using System.Linq;

[GlobalClass]
public partial class Inventory : Resource
{
    [Export] public InventorySlot[] Slots { get; set; }
    [Signal] public delegate void UpdateEventHandler();
    [Signal] public delegate void Update2EventHandler();

    public void Insert(InventoryItem item, int amount = 1)
    {
        if (item == null) return;

        // Find slots that already have this item
        var itemSlots = Slots.Where(slot => slot.item == item).ToArray();

        if (itemSlots.Length > 0)
        {
            itemSlots[0].amount += amount;
        }
        else
        {
            // Find empty slots
            var emptySlots = Slots.Where(slot => slot.item == null).ToArray();

            if (emptySlots.Length > 0)
            {
                emptySlots[0].item = item;
                emptySlots[0].amount = amount;
            }
        }

        EmitSignal(SignalName.Update);
    }

    public void RemoveAt(int index, int amount = 1)
    {
        if (Slots[index].amount > 1)
        {
            Slots[index].amount -= amount;
        }
        else
        {
            Slots[index].item = null;
        }

        EmitSignal(SignalName.Update2);
    }

    public void Remove(InventoryItem item, int amount = 1)
    {
        if (item == null) return;

        // Find slots that already have this item
        var itemSlots = Slots.Where(slot => slot.item == item).ToArray();

        if (itemSlots.Length > 0)
        {
            itemSlots[0].amount -= amount;
        }

        EmitSignal(SignalName.Update2);
    }
}
