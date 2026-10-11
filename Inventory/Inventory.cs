using Godot;
using System;
using System.ComponentModel;
using System.Linq;

[GlobalClass]
public partial class Inventory : Resource
{
    [Export] public InventorySlot[] slots { get; set; }
    [Signal] public delegate void UpdateEventHandler();
    [Signal] public delegate void Update2EventHandler();

    public void Insert(InventoryItem item, int amount = 1)
    {
        if (item == null) return;

        // Find slots that already have this item
        var itemSlots = slots.Where(slot => slot.item == item).ToArray();

        if (itemSlots.Length > 0)
        {
            itemSlots[0].amount += amount;
        }
        else
        {
            // Find empty slots
            var emptySlots = slots.Where(slot => slot.item == null).ToArray();

            if (emptySlots.Length > 0)
            {
                emptySlots[0].item = item;
                emptySlots[0].amount = amount;
            }
        }

        EmitSignal(SignalName.Update);
    }

    public void removeAt(int index, int amount = 1)
    {
        if (slots[index].amount > 1)
        {
            slots[index].amount -= amount;
        }
        else
        {
            slots[index].item = null;
        }

        EmitSignal(SignalName.Update2);
    }

    public void remove(InventoryItem item, int amount = 1)
    {
        if (item == null) return;

        // Find slots that already have this item
        var itemSlots = slots.Where(slot => slot.item == item).ToArray();

        if (itemSlots.Length > 0)
        {
            itemSlots[0].amount -= amount;
        }

        EmitSignal(SignalName.Update2);
    }
}
