using Godot;
using System;

public partial class InteractableArea : Area3D
{
    [Export] public string interaction { get; set; } = "";
    [Export] public string scenePath { get; set; } = "";
    [Export] public Vector3 PlayerExitPosition; // Can set in editor so the player exits at the correct position when leaving the entered scene
    [Export] public Vector3 PlayerExitRotation; 
    private ShapeCast3D interactShapeCast;
    private bool inInteractableArea;
	private Label prompt;

	public override void _Ready()
    {
        interactShapeCast = GetNode<ShapeCast3D>("../CharacterBody3D/HeadOriginalPosition/Head/CameraSmooth/Camera3D/InteractShapeCast3D");
		prompt = GetNode<Label>("../CharacterBody3D/Control/Dithering/InteractionPrompt");
		prompt.Visible = false;
	}
    
    public override void _PhysicsProcess(double delta)
    {
        bool isInArea = IsShapeCastInArea();
        
        if (isInArea)
        {
            HandleShapeCastEntered();
        }
        else if (!isInArea)
        {
            HandleShapeCastExited();
        }
    }
    
    private bool IsShapeCastInArea()
    {
        if (interactShapeCast == null || !interactShapeCast.IsColliding())
            return false;
        
        for (int i = 0; i < interactShapeCast.GetCollisionCount(); i++)
        {
            Node3D collider = interactShapeCast.GetCollider(i) as Node3D;
            if (collider == this)
            {
                return true;
            }
        }
        
        return false;
    }

    private void HandleShapeCastEntered()
    {
        inInteractableArea = true;
        prompt.Visible = true;

        if (Input.IsActionJustPressed("interact"))
        {
            if (interaction == "Change Scene")
            {
                GlobalScript.Instance.PlayerTownPosition = PlayerExitPosition;
                GlobalScript.Instance.PlayerTownRotation = PlayerExitRotation;
                prompt.Visible = false;
                GetTree().ChangeSceneToFile(scenePath);
            }
        }
    }

    private void HandleShapeCastExited()
    {
        if (inInteractableArea)
        {
            prompt.Visible = false;
            inInteractableArea = false;
        }
    }
}