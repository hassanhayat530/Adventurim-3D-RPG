using Godot;

public partial class MainMenu : Control
{
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

    private async void OnStartPressed()
    {
        sceneTransition.changeScene("res://Scenes/CharacterCreation.tscn");
    }

    private void OnContinuePressed()
    {
        GlobalScript.Instance.LoadGame();
        sceneTransition.changeScene("res://Scenes/townDay.tscn");
    }

    private void OnSettingsPressed()
    {
        GD.Print("Settings pressed");
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    private async void OnScenarioGenerationPressed()
    {
        await GetNode<LLMService>("/root/LLMService").TriggerScenario(GetTree().Root);
    }
    
}