using Godot;
using System;

public partial class AcceptButton : Label
{
	[Export] private LineEdit name;
	private SceneTransitionBasic sceneTransition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		sceneTransition = GetNode<SceneTransitionBasic>("SceneTransition");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed()
	{
		if (name.Text.Length > 0)
		{
			GlobalScript.Instance.PlayerName = name.Text;
			GlobalScript.Instance.UpdateMaxHealth();
			GlobalScript.Instance.PlayerHealth = GlobalScript.Instance.PlayerMaxHealth;
			GlobalScript.Instance.UpdateMaxMana();
			GlobalScript.Instance.PlayerMana = GlobalScript.Instance.PlayerMaxMana - 50;
			GD.Print("Max Mana: " + GlobalScript.Instance.PlayerMaxMana);
			GD.Print("Mana: " + GlobalScript.Instance.PlayerMana);

			GlobalScript.Instance.DayChanged = true;
			sceneTransition.changeScene("res://Scenes/Town.tscn", true);
		}
	}
}
