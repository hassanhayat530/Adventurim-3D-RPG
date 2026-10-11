using Godot;
public partial class RandomVisibility : Node3D
{
    [Export(PropertyHint.Range, "0,1")]
    public float spawnChance { get; set; } = 0.5f;
    [Export] public RandomVisibility[] conflictingObjects = [];
    public override void _Ready()
    {
        Visible = GD.Randf() < spawnChance;

        if (!Visible)
        {
            QueueFree();
            return;
        }

        if (Visible && conflictingObjects != null)
        {
            foreach (var obj in conflictingObjects)
            {
                obj.Visible = false;
            }
        }
    }
}