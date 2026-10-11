# Adventurim

An early prototype of a 3D dungeon-crawler RPG built in Godot with C#. It is inspired by the visuals and gameplay of old-school RPGs, with a retro look created through pixelation and dithering shaders.

**Download the latest build on itch.io:** [Adventurim](https://darthadox.itch.io/adventurim)

> This is a work in progress. Some features described below, such as levelling up and dungeon loot, are planned but not yet in the current build.

## Gameplay Loop

1. Spawn in town at night, sleep at the inn to restore health and mana, and buy or sell equipment at the merchant.
2. Level up at the inn when you have enough experience (planned).
3. At day, head out into a procedurally generated dungeon.
4. Fight monsters, find loot, and gain experience (planned).
5. Return to town and repeat.

## Features

- **Character creation:** name your character and pick a class.
  - **Soldier:** favours survivability and damage
  - **Rogue:** favours speed and hit chance
  - **Sorcerer:** favours magic ability, with some strength in several areas
- **Town hub:** a medieval town with an inn (spend the night to advance the day) and a merchant's shop. The inn is the largest building in the centre of the street, and the shop is across from it on the left.
- **Merchant:** buy items from the merchant's inventory or sell items from your own.
- **Inventory:** 24 slots filled at the first free space. Hover over an item to see its stats, and click equipment to equip it in the matching slot.
- **Class-based loot:** light armor for rogues and sorcerers, heavy armor for soldiers, and daggers, staffs, and swords for rogues, sorcerers, and soldiers respectively. Merchant stock improves with player level.
- **Dungeons:** an early low-level dungeon that can be explored, with no loot or monsters yet.
- **Scenario generation (experimental):** a preview button in the main menu that generates a scenario with an AI model. It needs an API key in your system environment variables (tested with Claude). Once started, the only way to leave it is to close the game.

## Controls

| Key | Action |
|---|---|
| `W` `A` `S` `D` | Move |
| `Space` | Jump |
| `Shift` | Sprint |
| `Ctrl` | Crouch |
| Arrow keys | Look around |
| `E` | Interact and open doors |
| `I` | Open and close inventory |
| Left mouse button | Attack |
| `R` | Sheathe weapon |
| `Esc` | Pause menu (only Resume works for now) |
| `V` | Toggle noclip (debug) |

The inventory can be opened at any time except inside the inn or the merchant's shop.

## Running the Project

1. Install the .NET version of [Godot 4.4](https://godotengine.org/download) and the .NET SDK. The project uses the Forward+ renderer.
2. Clone the repository and import `project.godot` in the Godot project manager.
3. Let Godot import the assets. The `.godot` cache folder is not stored in the repository, so the first import can take a moment.
4. Build the C# solution (Build button in the editor) and press Play.

All assets are stored in the `assets` folder in the root of the repository.

## Technical Overview

- **3D environments:** the hub and all dungeon rooms are made in [TrenchBroom](https://trenchbroom.github.io/) as Quake-style `.map` files, imported through the func_godot plugin. It is more limited than Blender, which suits the retro style.
- **Shaders:** a pixelation shader makes the game look low resolution without lowering the actual resolution, and a dithering shader uses a Bayer matrix and a reduced palette for the old-school look.
- **Scripts:** written in C# rather than GDScript. Most use `_Ready` for setup, and `_Process` is kept to a minimum to reduce per-frame load.
- **Global script:** stores frequently accessed values such as player attributes, the current day, and equipment.
- **Custom resources:**
  - `InventoryItem`: items created in the editor with attributes based on item type, so new items need no new script
  - `ItemSpawn`: an array of `InventoryItem` entries with spawn chances
  - `InventorySlot`: an item and its amount
  - `Inventory`: an array of slots with insert and remove functions that emit signals for the UI to update
- **Item spawn generation:** items are picked by weighted random selection. A random value between 0 and the total spawn chance is generated, and the item whose cumulative chance first reaches that value is chosen. Chosen items are removed from the pool to avoid duplicates, and the process repeats until enough items are selected.

## Roadmap

- Develop scenario generation and bring it into the dungeons
- Show a random scenario screen where the player responds with one of several choices
- Show the consequences in the 3D world or on the character, such as spawning an enemy, gaining or losing stats, or finding loot
- Add monsters and loot to the low-level dungeon and replace the blue placeholder rooms

## Credits

This project uses the following third-party assets and tools. All rights belong to their creators.

### Plugins, code, and shaders

- [SimpleDungeons and SimpleFPSController](https://github.com/majikayogames) by majikayogames
- [func_godot plugin](https://github.com/func-godot)
- [Arbitrary Color Reduction and Palette Ordered Dithering](https://godotshaders.com/shader/arbitrary-color-reduction-ordered-dithering)
- [Brackeys' VFX Bundle](https://brackeysgames.itch.io/brackeys-vfx-bundle) by Brackeys

### Models, textures, and effects

- [Tiny Texture Pack 1](https://screamingbrainstudios.itch.io/tiny-texture-pack) by Screaming Brain Studios
- [900+ Pixel Art Texture - 256x256](https://flakdeau19.itch.io/pixel-art-texture-pack-256x256) by flakdeau19
- [voidpix skybox](https://abhishta.itch.io/voidpix-skybox-pack) by abhishta
- [Texture Pack: Leather and Metal](https://juliovii.itch.io/ftp-leather-metal) by JulioVII
- [PSX Dungeon](https://goblinatron.itch.io/psx-dungeon) by Goblinatron
- [PSX - Dark Fantasy Medieval Pack](https://wildenza.itch.io/psx-medieval-pack) by Wildenza
- [Medieval Weapons Asset Pack](https://wildenza.itch.io/medieval-weapons-asset-pack) by Wildenza
- [PSX First Person Arms](https://drillimpact.itch.io/psx-first-person-arms-free) by Drillimpact
- [Izchak's Torches](https://opengameart.org/content/izchaks-torches) by cemkalyoncu
- Medieval Street Lantern by Alexey.inc (Sketchfab)
- [Lantern](https://sketchfab.com/3d-models/lantern-b96cffa4485449b1b9d53afe26b1e4b8) by VladimirKrysenko
- [Wand](https://vulpessoftware.itch.io/asset-wand) by Vulpes Software

### User interface

- [25 Pixel Art Portraits](https://medievalmore.itch.io/25-pixel-art-portraits-1) by MedievalMore
- [Custom Border and Panels Menu All Part](https://bdragon1727.itch.io/custom-border-and-panels-menu-all-part) by BDragon1727
- [Complete UI Book Styles Pack](https://crusenho.itch.io/complete-ui-book-styles-pack) by Crusenho Agus Hennihuno
- [Crimson Fantasy User Interface](https://byandrox.itch.io/crimson-fantasy-gui) by Androx
- [Health Bar Asset Pack 2](https://adwitr.itch.io/pixel-health-bar-asset-pack-2) by Adwit Rahman
- [Humble Gift v1.1: Player UI](https://humblepixel.itch.io/pocket-inventory-series-5-player-status/devlog/955031/-humble-gift-v11-player-ui-) by Humble Pixel
- [Super Asset Bundle #2: Adventure Time](https://humblepixel.itch.io/super-asset-bundle-2-adventure-time) by Humble Pixel

### Fonts and audio

- [Alagard](https://www.dafont.com/hewett-tsoi.d4888) font by Hewett Tsoi
- [My Dearest Bit](https://hyohnoo.itch.io/mydearestbit) font by Hyohnoo
- Soliloquy by [Matthew Pablo](https://opengameart.org/users/matthew-pablo)
- Silence Unbroken I & II by [Lost in The Forest](https://lostintheforest.bandcamp.com)
- SFX by [artisticdude](https://opengameart.org/users/artisticdude)
