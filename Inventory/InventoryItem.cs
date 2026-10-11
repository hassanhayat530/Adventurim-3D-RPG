using Godot;
using Microsoft.VisualBasic;
using System;

[GlobalClass]
public partial class InventoryItem : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public string Type { get; set; } = "";
    [Export] public string WeaponType { get; set; } = "";
    [Export] public string ArmorType { get; set; } = "";
    [Export] public int SpeedDebuff { get; set; } = 0;
    [Export] public string ConsumableType { get; set; } = "";
    [Export] public int Effect { get; set; } = 0;
    [Export] public int Value { get; set; } = 0;
    [Export] public int SpawnChance { get; set; } = 0;
    [Export] public Texture2D Texture { get; set; }
    [Export] public PackedScene Scene { get; set; }
    public InventoryItem() { }
}