using Godot;
using System;

public partial class Hand : Node3D
{
	private PackedScene scene;

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
}
