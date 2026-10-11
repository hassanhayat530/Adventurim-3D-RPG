using Godot;
using System;

public partial class Hand : Node3D
{
	private PackedScene scene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void equip(PackedScene weaponScene)
	{
		unequip();
		scene = weaponScene;
		var instance = scene.Instantiate<Weapon>();
		AddChild(instance);
	}

	public void unequip()
	{
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}

		scene = null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
