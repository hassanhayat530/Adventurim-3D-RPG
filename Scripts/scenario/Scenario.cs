using Godot;

public partial class Scenario : Control
{
    [Export] private RichTextLabel _narrativeLabel;
    [Export] private VBoxContainer _choicesContainer;

    private LLMService.ScenarioData scenarioData;

    public void DisplayScenario(LLMService.ScenarioData scenario)
    {
        scenarioData = scenario;
        _narrativeLabel.Text = scenarioData.Narrative;

        foreach (Node child in _choicesContainer.GetChildren())
            child.QueueFree();

        foreach (var choice in scenarioData.Choices)
        {
            Button btn = new Button();
            btn.Text = choice.Label;
            btn.Pressed += () => OnChoiceSelected(choice.Id);
            _choicesContainer.AddChild(btn);
        }
    }

    private async void OnChoiceSelected(string choiceId)
    {
        GD.Print("Player picked: " + choiceId);

        LLMService.ScenarioResolution requestResolution = await GetNode<LLMService>("/root/LLMService").RequestResolution(scenarioData, choiceId);
        
        GD.Print(requestResolution.ResolutionText);
        GD.Print("Effect Type = " + requestResolution.Effect.Type);
        GD.Print("Enemy ID = " + requestResolution.Effect.EnemyId);
        GD.Print("Item ID = " + requestResolution.Effect.ItemId);
        GD.Print("Stat = " + requestResolution.Effect.Stat);
        GD.Print("Amount = " + requestResolution.Effect.Amount);
    }
}