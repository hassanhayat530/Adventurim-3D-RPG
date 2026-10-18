using Godot;
using System;

public partial class SceneTransitionBasic : CanvasLayer
{
	[Export] public bool displayDayOnReady { get; set; } = false;
	private AnimationPlayer animationPlayer;
	private ColorRect colorRect;
	private Label day;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		colorRect = GetNode<ColorRect>("ColorRect");
		day = GetNode<Label>("Label");

		day.Text = "Day " + GlobalScript.Instance.Day.ToString();

		if (GlobalScript.Instance.DayChanged)
		{
			day.Visible = true;
		}
		else
		{
			day.Visible = false;
		}

		Visible = true;

		fadeOut();
	}

	public async void fadeOut()
	{
		animationPlayer.Play("FadeOut");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		Visible = false;
	}

	public async void changeScene(string scenePath, bool displayDay = false)
	{

		if (displayDay) day.Visible = true; else day.Visible = false;

		Visible = true;
		animationPlayer.Play("FadeIn");

		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

		var err = GetTree().ChangeSceneToFile(scenePath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
