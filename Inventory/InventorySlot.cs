using Godot;
using System;

[GlobalClass]
public partial class InventorySlot : Resource
{
    [Export] public InventoryItem item;
    [Export] public int amount;
    [Export] public bool equipped;
}
