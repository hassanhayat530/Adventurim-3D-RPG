using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class GlobalScript : Node
{
    public static GlobalScript Instance { get; private set; }

    public int Day { get; set; } = 1;
    public bool isNight { get; set; } = true;

    public bool DayChanged { get; set; } = false;
    public Dictionary<string, int> attributes = new Dictionary<string, int>
    {
        {"Constitution", 10},
        {"Strength", 15},
        {"Dexterity", 25},
        {"Intelligence", 10},
        {"Charisma", 20},
        {"Speed", 25}
    };
    public Dictionary<string, InventoryItem> Equipment = new Dictionary<string, InventoryItem>
    {
        {"Head", null},
        {"Chest", null},
        {"Legs", null},
        {"Hands", null},
        {"Weapon", null},
        {"Ring1", null},
        {"Ring2", null}
    };

    public float PlayerSellingRatio;
    public float PlayerBuyingRatio;
    public Inventory PlayerInventory = GD.Load<Inventory>("res://Inventory/PlayerInventory.tres");
    public Inventory MerchantInventory = GD.Load<Inventory>("res://Inventory/StoreInventory.tres");
    public ItemSpawn MerchantLowLevelSpawn = GD.Load <ItemSpawn>("res://Inventory/ItemSpawns/LowLevelMerchantItems.tres");
    public bool InInventory { get; set; } = false;
    public string PlayerName { get; set; } = "Player";
    public string PlayerClass { get; set; } = "Soldier";
    public string PlayerSex { get; set; } = "Male";

    public int PlayerLevel { get; set; } = 1;
    public int PlayerXP { get; set; } = 0;
    public int NextXPGoal { get; set; } = 1000;

    public int PlayerGold { get; set; } = 0;

    public Texture2D PlayerPortrait { get; set; }
    public Texture2D PlayerPortraitBorder { get; set; }

    public float PlayerMaxHealth { get; set; }
    public float PlayerHealth { get; set; }
    public float PlayerMaxMana { get; set; }
    public float PlayerMana { get; set; }

    public Godot.Vector3 PlayerTownPosition { get; set; } = new Godot.Vector3(3.9f, 1f, 39.8f);
    public Godot.Vector3 PlayerTownRotation { get; set; } = new Godot.Vector3(0f, 0f, 0f);

    public override void _Ready()
    {
        List<InventoryItem> merchantItems = GenerateItemSpawn(MerchantLowLevelSpawn, 6);
        for (int i = 0; i < merchantItems.Count; ++i)
        {
            MerchantInventory.Insert(merchantItems[i]);
        }
    }

    // Called when script enters the tree
    public override void _EnterTree()
    {
        if (Instance != null)
        {
            QueueFree(); // Prevent duplicates
            return;
        }
        Instance = this;
    }

    public void UpdateAttributes(string[] values)
    {
        attributes["Constitution"] = int.Parse(values[0]);
        attributes["Strength"] = int.Parse(values[1]);
        attributes["Dexterity"] = int.Parse(values[2]);
        attributes["Intelligence"] = int.Parse(values[3]);
        attributes["Charisma"] = int.Parse(values[4]);
        attributes["Speed"] = int.Parse(values[5]);
    }

    public void UpdateMaxHealth()
    {
        PlayerMaxHealth = attributes["Constitution"] * 10;
    }

    public void UpdateMaxMana()
    {
        PlayerMaxMana = attributes["Intelligence"] * 10;
    }

    public void updateEquipment()
    {
        // Get refererence to the equipment node in the nested inventory scene
        Node currentScene = GetTree().CurrentScene;
        Node characterScene = currentScene.GetNode<Node>("CharacterBody3D");
        Node inventoryScene = currentScene.GetNode<Node>("Control/Dithering/InventoryUI");
        Control equipment = inventoryScene.GetNode<Control>("Equipment");

        // Get non-null children of equipment node (EquipmentSlots)
        EquipmentSlot[] equipmentSlots = equipment.GetChildren()
                                    .Select(child => child as EquipmentSlot)
                                    .Where(slot => slot != null)
                                    .ToArray();

        // Fill all slot types with the martching type held in the dictionary
        for (int i = 0; i < equipmentSlots.Length; ++i)
        {
            EquipmentSlot currEquipmentSlot = equipmentSlots[i];
            currEquipmentSlot.fill(Equipment[currEquipmentSlot.equipmentType]);
        }
    }


    // Takes InventoryItems kept in an ItemSpawn resource and returns a list of required InventoryItems based on spawn chance of each.
    public List<InventoryItem> GenerateItemSpawn(ItemSpawn itemSpawn, int numItems)
    {
        List<InventoryItem> selectedItems = [];
        List<InventoryItem> availableItems = itemSpawn.items.ToList();

        for (int i = 0; i < numItems; ++i)
        {
            if (availableItems.Count == 0)
            {
                break;
            }

            // Get total chance to use for weight distibution of individual items
            float totalChance = 0;
            foreach (InventoryItem item in availableItems)
            {
                totalChance += item.SpawnChance;
            }

            // Generate a random value between 0 and totalChance
            float randomValue = (float)GD.RandRange(0, totalChance);

            // Find which item corresponds to the random value
            // Create a cumulative item chance line
            float cumulativeChance = 0;
            InventoryItem selectedItem = null;

            // Select item present on the approapriate position on the cumulative chance line
            // Items with a bigger spawn chance will occupy more space on the cumulative chance line
            foreach (InventoryItem item in availableItems)
            {
                cumulativeChance += item.SpawnChance;
                if (randomValue <= cumulativeChance)
                {
                    selectedItem = item;
                    break;
                }
            }

            if (selectedItem != null)
            {
                selectedItems.Add(selectedItem);
                availableItems.Remove(selectedItem);
            }
        }

        return selectedItems;
    }

    public void SaveGame(string saveName = "savegame.tres") {
    // Create a new save game resource
    var saveGame = new SaveGame();
    
    // Copy data from GlobalScript to saveGame
    saveGame.Day = Day;
    saveGame.IsNight = isNight;
    saveGame.PlayerLevel = PlayerLevel;
    saveGame.PlayerXP = PlayerXP;
    saveGame.NextXPGoal = NextXPGoal;
    saveGame.PlayerGold = PlayerGold;
    saveGame.PlayerPortrait = PlayerPortrait;
    saveGame.PlayerPortraitBorder = PlayerPortraitBorder;
    saveGame.PlayerMaxHealth = PlayerMaxHealth;
    saveGame.PlayerHealth = PlayerHealth;
    saveGame.PlayerMaxMana = PlayerMaxMana;
    saveGame.PlayerMana = PlayerMana;
    saveGame.PlayerName = PlayerName;
    saveGame.PlayerClass = PlayerClass;
    saveGame.PlayerSex = PlayerSex;
    saveGame.PlayerTownPosition = PlayerTownPosition;
    saveGame.PlayerTownRotation = PlayerTownRotation;
    
    // Convert dictionaries to Godot collections
    saveGame.Attributes = new Godot.Collections.Dictionary<string, int>();
    foreach (var kvp in attributes)
    {
        saveGame.Attributes[kvp.Key] = kvp.Value;
    }
    
    saveGame.Equipment = new Godot.Collections.Dictionary<string, InventoryItem>();
    foreach (var kvp in Equipment)
    {
        saveGame.Equipment[kvp.Key] = kvp.Value;
    }
    
    // Save inventory
    saveGame.PlayerInventory = PlayerInventory;
    
    // Save the resource
    string savePath = $"user://{saveName}";
    Error error = ResourceSaver.Save(saveGame, savePath);
    
    if (error == Error.Ok)
    {
        GD.Print($"Game saved successfully to {savePath}");
    }
    else
    {
        GD.PrintErr($"Failed to save game: {error}");
    }
}

public void LoadGame(string saveName = "savegame.tres")
{
    string savePath = $"user://{saveName}";
    
    // Check if save file exists
    if (!FileAccess.FileExists(savePath))
    {
        GD.PrintErr($"Save file not found: {savePath}");
        return;
    }
    
    // Load the save game resource
    var saveGame = GD.Load<SaveGame>(savePath);
    
    if (saveGame == null)
    {
        GD.PrintErr("Failed to load save game");
        return;
    }
    
    // Restore data from saveGame to GlobalScript
    Day = saveGame.Day;
    isNight = saveGame.IsNight;
    PlayerLevel = saveGame.PlayerLevel;
    PlayerXP = saveGame.PlayerXP;
    NextXPGoal = saveGame.NextXPGoal;
    PlayerGold = saveGame.PlayerGold;
    PlayerPortrait = saveGame.PlayerPortrait;
    PlayerPortraitBorder = saveGame.PlayerPortraitBorder;
    PlayerMaxHealth = saveGame.PlayerMaxHealth;
    PlayerHealth = saveGame.PlayerHealth;
    PlayerMaxMana = saveGame.PlayerMaxMana;
    PlayerMana = saveGame.PlayerMana;
    PlayerName = saveGame.PlayerName;
    PlayerClass = saveGame.PlayerClass;
    PlayerSex = saveGame.PlayerSex;
    PlayerTownPosition = saveGame.PlayerTownPosition;
    PlayerTownRotation = saveGame.PlayerTownRotation;
    
    // Restore dictionaries
    attributes.Clear();
    foreach (var kvp in saveGame.Attributes)
    {
        attributes[kvp.Key] = kvp.Value;
    }
    
    Equipment.Clear();
    foreach (var kvp in saveGame.Equipment)
    {
        Equipment[kvp.Key] = kvp.Value;
    }
    
    // Restore inventory
    PlayerInventory = saveGame.PlayerInventory;
    
    // Recalculate derived values
    UpdateMaxHealth();
    UpdateMaxMana();
    
    GD.Print("Game loaded successfully");
}
}
