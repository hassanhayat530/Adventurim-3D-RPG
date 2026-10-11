using Godot;
using System;
using System.Xml.Serialization;

public partial class SceneTransition : CanvasLayer
{
	public static SceneTransition Instance { get; private set; }
	[Export] private float switchDuration = 1f;
	public string currentScene = "";

	private ColorRect colorRect;
	// Called when the node enters the scene tree for the first time.

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            QueueFree(); // Prevent duplicates
            return;
        }
        Instance = this;
    }

	public SceneTransition()
	{
		RegEx regex = new();
		regex.Compile("res:\\/\\/scenes\\/([^\\.]+)");
		RegExMatch result = regex.Search((string)
			ProjectSettings.GetSetting("application/run/main_scene"));
		currentScene = result.GetString(1);
	}
	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
		colorRect.Modulate = new Color(1, 1, 1, 0);
	}

	public async void GoToScene(string scene)
	{
		colorRect.MouseFilter = Control.MouseFilterEnum.Stop;

		Tween tween = GetTree().CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "modulate",
			new Color(1, 1, 1, 1), switchDuration / 2f);
		await ToSignal(tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToFile($"res://scenes/{scene}.tscn");
		GetTree().Paused = false;
		currentScene = scene;

		tween = GetTree().CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "modulate",
			new Color(1, 1, 1, 0), switchDuration / 2f);
		colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
