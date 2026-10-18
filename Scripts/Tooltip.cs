using Godot;

public partial class Tooltip : PanelContainer
{
    private static Vector2 OFFSET = Vector2.One * 60f;
    private Tween opacityTween = null;
    
    // Reference to the RichTextLabel child
    private RichTextLabel richTextLabel;

    public override void _Ready()
    {
        richTextLabel = GetNode<RichTextLabel>("RichTextLabel"); // Adjust path as needed
        Hide();
        // ProcessMode = ProcessModeEnum.WhenPaused;
    }

    // Method to set tooltip content
    public void SetContent(string content)
    {
        if (richTextLabel == null) return;
        richTextLabel.Text = content;
        Callable.From(ResetSize).CallDeferred(); // shrink/grow panel to the new content
    }

    public override void _Input(InputEvent @event)
    {
        if (Visible && @event is InputEventMouseMotion)
        {
            GlobalPosition = GetGlobalMousePosition() + OFFSET;
        }
    }

    public async void Toggle(bool on, string content = null)
    {
        if (on)
        {
            // Set content before showing
            if (!string.IsNullOrEmpty(content))
            {
                SetContent(content);
            }

            Show();
            Modulate = new Color(1, 1, 1, 0);
            TweenOpacity(new Color(1, 1, 1, 1));
        }
        else
        {
            Modulate = new Color(1, 1, 1, 1);
            await ToSignal(TweenOpacity(new Color(1, 1, 1, 0)), Tween.SignalName.Finished);
            Hide();
        }
    }

    public Tween TweenOpacity(Color to)
    {
        opacityTween?.Kill();
        opacityTween = GetTree().CreateTween();
        opacityTween.TweenProperty(this, "modulate", to, 0.3);
        return opacityTween;
    }
}