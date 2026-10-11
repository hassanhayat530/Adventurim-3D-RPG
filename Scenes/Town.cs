using Godot;
using System;

public partial class Town : Node3D
{
	GlobalScript globalScript;
	CharacterBody3D player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		globalScript = GlobalScript.Instance;

		player = GetNode<CharacterBody3D>("CharacterBody3D");
		player.Position = globalScript.PlayerTownPosition;
		player.Rotation = globalScript.PlayerTownRotation;

		globalScript.PlayerSellingRatio = (float)(0.5 + (globalScript.attributes["Charisma"] / 100f));
		globalScript.PlayerBuyingRatio = (float)(1.5f - (globalScript.attributes["Charisma"] / 100f));

		globalScript.DayChanged = false;

		// if (!globalScript.InTown)
		// {
		// 	GetNode<SceneTransitionBasic>("SceneTransition").displayDayOnReady = true;
		// }
		// else
        // {
		// 	globalScript.InTown = true;
        // }
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
