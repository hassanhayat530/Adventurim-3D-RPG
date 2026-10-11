// using Godot;
// using System;

// public partial class RpgGameController : Node
// {
// 	[Export] private RichTextLabel scenarioText;
//     [Export] private VBoxContainer optionsContainer;
//     [Export] private Label healthLabel;
//     [Export] private TextEdit inventoryText;
//     [Export] private Label loadingLabel;  // Optional
// 	// Called when the node enters the scene tree for the first time.

// 	private ILLMService llmService;
//     private PlayerState playerState;
// 	public override void _Ready()
// 	{
// 		llmService = new LocalLLMService();
//         playerState = new PlayerState();
// 	}

// 	private void StartNewScenario()
//     {
//         // Example scenario
//         var scenario = new GameScenario // new class created in another script
//         {
//             Description = "[b]You stand before an ancient temple.[/b]\nThe doors are covered in strange runes.",
//             Options = new List<PlayerOption> // PlayerOption will also be a new class
//             {
//                 new() { Text = "Touch the runes", ActionType = "damage", Value = 10 },
//                 new() { Text = "Look for another entrance", ActionType = "item", Value = 1 }
//             }
//         };
        
//         DisplayScenario(scenario);
//     }

// 	private void DisplayScenario(GameScenario scenario)
//     {
//         scenarioText.Text = scenario.Description;
        
//         // Clear old buttons
//         foreach (Node child in optionsContainer.GetChildren())
//             child.QueueFree();
        
//         // Add new buttons
//         for (int i = 0; i < scenario.Options.Count; i++)
//         {
//             var button = new Button();
//             button.Text = scenario.Options[i].Text;
//             button.Pressed += () => OnOptionSelected(i);
//             optionsContainer.AddChild(button);
//         }
        
//         UpdatePlayerUI();
//     }

// 	// Called every frame. 'delta' is the elapsed time since the previous frame.
// 	public override void _Process(double delta)
// 	{
// 	}
// }
