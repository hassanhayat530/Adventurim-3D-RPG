using Godot;
using System;

public partial class TorchTrigger : MeshInstance3D
{
	private Node3D _vfx;
	private Area3D _trigger;

	public override void _Ready()
	{
		_vfx = GetNode<Node3D>("vfx");
		_trigger = GetNode<Area3D>("Trigger");

		_trigger.BodyEntered += OnBodyEntered;
	}

    private void OnBodyEntered(Node3D body)
    {
		GD.Print($"Entered: {body.Name}");
		if (body is not CharacterBody3D) return;
        _vfx.Visible = true;
    }
}
