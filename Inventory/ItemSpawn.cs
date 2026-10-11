using Godot;
using System;

[GlobalClass]
public partial class ItemSpawn : Resource
{
    [Export] public InventoryItem[] items { get; set; }

    public ItemSpawn() { }
}
