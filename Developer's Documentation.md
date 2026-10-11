## 3D Environments

The project utilizes a tool called Trenchbroom to create retro 3D .map files. The hub area within the game as well as all the dungeon rooms have been created using this tool. Decision was made to use this instead of blender because trenchbroom is used to make quake style maps. This means it is more limited than tools like blender but perfect for the intended retro style of the game.

## Shaders

The project utilizes two shaders to achive it's old school look in a 3D environment.
- Pixelation: It makes the game looks like it is running on low resolution without actually lowering the resolution
- Dithering: Old school games used to have reduced color palettes and utilizes a bayer matrix to add noise to the game. This is essential for the retro look of the game

## Scripts

The project makes use of  C# scripts rather than gdscripts. This decision was made in regards to the familiarity of the language as well as some former work on unity which also utilizes C#. Each script contains two essential functions:
- _Ready: Runs when the scene tree is initialized including this node's children
- _Process: Runs per frame of the game. You may get a different framerate based on your device.
Due to the nature of _Process, not many scripts utilize it to reduce load.

## GlobalScript

The project utilizes a global script to store values that need to be accessed frequently by other scripts in the game such as player attributes, the current day number, player equipment etc.

## Custom Resources

- InventoryItem: This resource allows creation of multiple inventory items using the editor which can then be assigned different attributes based on the item type. This saves time as making new objects in the editor is faster than making a new script for each item.
- ItemSpawn: Contains an array of InventoryItems that have custom spawn chances (see `Generating Item Spawns`).
- InventorySlot: Contains an inventory slot. It hold an attribute for inventoryItem and the amount present.
- Inventory: Contains an array of inventorySlots. This is utilized when creating inventories in the game. It has simple a simple insert algorithm to insert an item in the first available space or add amount to an already present one. Both remove and insert functions work similarly and emit signals for update. The insert signal is attached to the InventoryUI script and the remove signal is attached to the MerchantUI script.

## Generating Item Spawns
The global script has an algorithm to generate a list of a custom number of items based on their spawn chance.
- Keep track of available items to avoid duplication
- Calculate total chance by adding all the chances in the ItemSpawn resource together.
- Generate a random value between 0 and total chance.
- Initialize a cumulative chance at 0
- for each item in available items, add their spawn chance to the cumulative chance. If the cumulative chance is more than or equal to the random value, we select it.
- Store the selected item in a list and repeat after generating a new random value until the required number of items is selected.
Think of this as spreading the items out on a number line based on chance. If an item has a higher chance, there will be more probability of the random value falling into it's space in the number line.
