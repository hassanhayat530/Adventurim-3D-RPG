using Godot;

public partial class Scenario : Control
{
	[Export] private Label _titleLabel;
	[Export] private RichTextLabel _narrativeLabel;
	[Export] private RichTextLabel _choiceSelectedLabel;
	[Export] private RichTextLabel _resolutionLabel;
	[Export] private VBoxContainer _choicesContainer;
	[Export] private AnimationPlayer _animationPlayer;

	private LLMService.ScenarioData scenarioData;

	public void DisplayScenario(LLMService.ScenarioData scenario)
	{
		_animationPlayer.Play("Open");
		scenarioData = scenario;
		_narrativeLabel.Text = scenarioData.Narrative;
		_titleLabel.Text = scenarioData.Title;

		foreach (Node child in _choicesContainer.GetChildren())
			child.QueueFree();

		for (int i = 0; i < scenarioData.Choices.Count; ++i)
		{
			LLMService.ScenarioChoice choice = scenarioData.Choices[i];

			Button btn = new();
			btn.Text = $"{i + 1}. {choice.Label}";
			btn.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
			btn.AddThemeStyleboxOverride("normal", new StyleBoxEmpty());
			btn.AddThemeStyleboxOverride("hover", new StyleBoxEmpty());
			btn.AddThemeStyleboxOverride("pressed", new StyleBoxEmpty());
			btn.AddThemeFontSizeOverride("font_size", 30);
			btn.AddThemeColorOverride("font_color", new Color(0.25f, 0f, 0.43f));
			btn.AddThemeColorOverride("font_hover_color", new Color(0.71f, 0f, 0.43f));
			btn.AddThemeColorOverride("font_pressed_color", new Color(0f, 0f, 0f));
			btn.AddThemeColorOverride("font_focus_color", new Color(0.71f, 0f, 0.43f));
			btn.AddThemeFontOverride("font", ResourceLoader.Load<Font>("res://fonts/alagard.ttf"));
			btn.Pressed += () => OnChoiceSelected(choice.Id, choice.Label);

			_choicesContainer.AddChild(btn);
		}
	}

	private async void OnChoiceSelected(string choiceId, string choiceText)
	{
		foreach (Node child in _choicesContainer.GetChildren())
			child.QueueFree();

		_choiceSelectedLabel.Visible = true;
		_choiceSelectedLabel.Text = $"> You {choiceText}...";

		GD.Print("Player picked: " + choiceId);

		LLMService.ScenarioResolution requestResolution = await GetNode<LLMService>("/root/LLMService").RequestResolution(scenarioData, choiceId);
		
		_resolutionLabel.Visible = true;
		_resolutionLabel.Text = requestResolution.ResolutionText;
		GD.Print(requestResolution.ResolutionText);
		GD.Print("Effect Type = " + requestResolution.Effect.Type);
		GD.Print("Enemy ID = " + requestResolution.Effect.EnemyId);
		GD.Print("Item ID = " + requestResolution.Effect.ItemId);
		GD.Print("Stat = " + requestResolution.Effect.Stat);
		GD.Print("Amount = " + requestResolution.Effect.Amount);
	}
}
