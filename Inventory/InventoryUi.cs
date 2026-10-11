using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class InventoryUi : Control
{
    private Inventory inventory = GD.Load<Inventory>("res://Inventory/PlayerInventory.tres");
    private NinePatchRect inventoryPanel;
    private Control equipmentPanel;
    private InvUISlot[] slots;
    private EquipmentSlot[] equipmentSlots;
    private AudioStreamPlayer2D openSound;
    private AnimationPlayer player;
    private AnimatedSprite2D bookSprite;
    private AnimatedSprite2D bookAppear;
    private bool isOpen = false;
    [Signal] public delegate void UpdateEventHandler();
    
    public override void _Ready()
    {
        inventoryPanel = GetNode<NinePatchRect>("NinePatchRect");
        equipmentPanel = GetNode<Control>("Equipment");
        inventoryPanel.Visible = false;
        equipmentPanel.Visible = false;

        var gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
        slots = gridContainer.GetChildren()
            .Select(child => child as InvUISlot)
            .Where(slot => slot != null)
            .ToArray();

        Control equipment = GetNode<Control>("Equipment");
        equipmentSlots = equipment.GetChildren()
                    .Select(child => child as EquipmentSlot)
                    .Where(slot => slot != null)
                    .ToArray();

        openSound = GetNode<AudioStreamPlayer2D>("OpenSound");
        // player = GetNode<AnimationPlayer>("AnimationPlayer");
        // player.Play("RESET");

        bookSprite = GetNode<AnimatedSprite2D>("BookSprite");
        bookAppear = GetNode<AnimatedSprite2D>("BookSprite/BookAppear");
        bookSprite.PlayBackwards("default");
        bookAppear.PlayBackwards("default");
        bookAppear.Visible = false;

        updateSlots();
        Visible = false;
    }

    public void updateSlots()
    {
        // Update Inventory
        int minSize = Math.Min(inventory.slots.Length, slots.Length);
        for (int i = 0; i < minSize; ++i)
        {
            slots[i].update(inventory.slots[i]);
        }

        // Update Equipment
        for (int i = 0; i < equipmentSlots.Length; ++i)
        {
            EquipmentSlot currEquipmentSlot = equipmentSlots[i];
            InventoryItem currequipment = GlobalScript.Instance.Equipment[currEquipmentSlot.equipmentType];
            if (currequipment != null)
            {
                currEquipmentSlot.fill(currequipment);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("i"))
        {
            if (isOpen)
                close();
            else
                open();
        }
    }

	public async Task open()
	{
        openSound.Play();
        inventory.Update += updateSlots;
		Visible = true;
		isOpen = true;
		// player.Play("open");

        bookSprite.Play("default");
        await ToSignal(bookSprite, "animation_finished");
        inventoryPanel.Visible = true;
        equipmentPanel.Visible = true;

        bookAppear.Visible = true;
        bookAppear.Play("default");
        await ToSignal(bookAppear, "animation_finished");

        updateSlots();

		Input.MouseMode = Input.MouseModeEnum.Visible;

		// Use process mode instead of pausing the entire tree
		// ProcessMode = ProcessModeEnum.WhenPaused;
		// GetTree().Paused = true;

		GlobalScript.Instance.InInventory = true;
    }

    public async void close()
    {
        openSound.Play();
        inventory.Update -= updateSlots;
        // player.PlayBackwards("open");
        // await ToSignal(player, "animation_finished");
        bookAppear.PlayBackwards("default");
        await ToSignal(bookAppear, "animation_finished");
        inventoryPanel.Visible = false;
        equipmentPanel.Visible = false;
        bookAppear.Visible = false;
        
        bookSprite.PlayBackwards("default");
        await ToSignal(bookSprite, "animation_finished");

        Visible = false;
        isOpen = false;
        // GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Reset process mode
        ProcessMode = ProcessModeEnum.Inherit;

        GlobalScript.Instance.InInventory = false;

        EmitSignal(SignalName.Update);
    }
}