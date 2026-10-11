using Godot;
using System;
using System.Collections.Generic;

public partial class SaveGame : Resource
{
    [Export] public int PlayerLevel { get; set; }
    [Export] public int PlayerXP { get; set; }
    [Export] public int NextXPGoal { get; set; }
    [Export] public int PlayerGold { get; set; }
    [Export] public Texture2D PlayerPortrait { get; set; }
    [Export] public Texture2D PlayerPortraitBorder { get; set; }
    [Export] public float PlayerMaxHealth { get; set; }
    [Export] public float PlayerHealth { get; set; }
    [Export] public float PlayerMaxMana { get; set; }
    [Export] public float PlayerMana { get; set; }
    [Export] public string PlayerName { get; set; }
    [Export] public string PlayerClass { get; set; }
    [Export] public string PlayerSex { get; set; }

    [Export] public Godot.Collections.Dictionary<string, int> Attributes { get; set; }
    [Export] public Godot.Collections.Dictionary<string, InventoryItem> Equipment { get; set; }
    [Export] public Inventory PlayerInventory { get; set; }
    [Export] public Vector3 PlayerTownPosition { get; set; }
    [Export] public Vector3 PlayerTownRotation { get; set; }

    [Export] public int Day { get; set; }
    [Export] public bool IsNight { get; set; }
}
