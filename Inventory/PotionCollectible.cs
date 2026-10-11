using Godot;
using System;
using System.Threading.Tasks;

public partial class PotionCollectible : StaticBody2D
{
    [Export]
    public InventoryItem Item { get; set; }
    
    private Node player = null;

    public async Task OnInteractableAreaBodyEnteredAsync(Node body)
    {
        if (body.HasMethod("player"))
        {
            player = body;
            PlayerCollect();
            await ToSignal(GetTree().CreateTimer(0.1), "timeout");
            QueueFree();
        }
    }
    
    private void PlayerCollect()
    {
        // player.Collect(Item);
    }
}
