using Godot;
using System;

public partial class InnUi : Control
{
	private SceneTransitionBasic sceneTransition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		sceneTransition = GetNode<SceneTransitionBasic>("SceneTransition");
	}

	public void OnLeavePressed()
	{
		sceneTransition.changeScene("res://Scenes/town.tscn");
	}
	
	public void OnNextDayPressed()
    {
		GlobalScript.Instance.Day += 1;
		GlobalScript.Instance.DayChanged = true;
		GlobalScript.Instance.isNight = false;
		GlobalScript.Instance.PlayerHealth = GlobalScript.Instance.PlayerMaxHealth;
		GlobalScript.Instance.PlayerMana = GlobalScript.Instance.PlayerMaxMana;

		GlobalScript.Instance.SaveGame();

		sceneTransition.displayDayOnReady = true;
		sceneTransition.changeScene("res://Scenes/TownDay.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
