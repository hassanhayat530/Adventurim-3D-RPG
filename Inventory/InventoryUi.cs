using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class InventoryUi : Control
{
    private Inventory _inventory = GD.Load<Inventory>("res://Inventory/PlayerInventory.tres");
    private NinePatchRect _inventoryPanel;
    private Control _equipmentPanel;
    private InvUISlot[] _slots;
    private EquipmentSlot[] _equipmentSlots;
    private AudioStreamPlayer2D _openSound;
    private AnimationPlayer _player;
    private AnimatedSprite2D _bookSprite;
    private AnimatedSprite2D _bookAppear;
    private bool _isOpen = false;
    [Signal] public delegate void UpdateEventHandler();
    
    public override void _Ready()
    {
        _inventoryPanel = GetNode<NinePatchRect>("NinePatchRect");
        _equipmentPanel = GetNode<Control>("Equipment");
        _inventoryPanel.Visible = false;
        _equipmentPanel.Visible = false;

        var gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
        _slots = gridContainer.GetChildren()
            .Select(child => child as InvUISlot)
            .Where(slot => slot != null)
            .ToArray();

        Control equipment = GetNode<Control>("Equipment");
        _equipmentSlots = equipment.GetChildren()
                    .Select(child => child as EquipmentSlot)
                    .Where(slot => slot != null)
                    .ToArray();

        _openSound = GetNode<AudioStreamPlayer2D>("OpenSound");
        // player = GetNode<AnimationPlayer>("AnimationPlayer");
        // player.Play("RESET");

        _bookSprite = GetNode<AnimatedSprite2D>("BookSprite");
        _bookAppear = GetNode<AnimatedSprite2D>("BookSprite/BookAppear");
        _bookSprite.PlayBackwards("default");
        _bookAppear.PlayBackwards("default");
        _bookAppear.Visible = false;

        UpdateSlots();
        Visible = false;
    }

    public void UpdateSlots()
    {
        // Update Inventory
        int minSize = Math.Min(_inventory.Slots.Length, _slots.Length);
        for (int i = 0; i < minSize; ++i)
        {
            _slots[i].UpdateSlot(_inventory.Slots[i]);
        }

        // Update Equipment
        for (int i = 0; i < _equipmentSlots.Length; ++i)
        {
            EquipmentSlot currEquipmentSlot = _equipmentSlots[i];
            InventoryItem currequipment = GlobalScript.Instance.Equipment[currEquipmentSlot.EquipmentType];
            if (currequipment != null)
            {
                currEquipmentSlot.Fill(currequipment);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("i"))
        {
            if (_isOpen)
                Close();
            else
                Open();
        }
    }

	public async Task Open()
	{
        _openSound.Play();
        _inventory.Update += UpdateSlots;
		Visible = true;
		_isOpen = true;
		// player.Play("open");

        _bookSprite.Play("default");
        await ToSignal(_bookSprite, "animation_finished");
        _inventoryPanel.Visible = true;
        _equipmentPanel.Visible = true;

        _bookAppear.Visible = true;
        _bookAppear.Play("default");
        await ToSignal(_bookAppear, "animation_finished");

        UpdateSlots();

		Input.MouseMode = Input.MouseModeEnum.Visible;

		// Use process mode instead of pausing the entire tree
		// ProcessMode = ProcessModeEnum.WhenPaused;
		// GetTree().Paused = true;

		GlobalScript.Instance.InInventory = true;
    }

    public async void Close()
    {
        _openSound.Play();
        _inventory.Update -= UpdateSlots;
        // player.PlayBackwards("open");
        // await ToSignal(player, "animation_finished");
        _bookAppear.PlayBackwards("default");
        await ToSignal(_bookAppear, "animation_finished");
        _inventoryPanel.Visible = false;
        _equipmentPanel.Visible = false;
        _bookAppear.Visible = false;
        
        _bookSprite.PlayBackwards("default");
        await ToSignal(_bookSprite, "animation_finished");

        Visible = false;
        _isOpen = false;
        // GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Reset process mode
        ProcessMode = ProcessModeEnum.Inherit;

        GlobalScript.Instance.InInventory = false;

        EmitSignal(SignalName.Update);
    }
}