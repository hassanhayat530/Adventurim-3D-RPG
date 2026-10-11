using Godot;

public partial class Weapon : Node3D
{
    [Export] public AnimationPlayer animationPlayer { get; set; }

    private const float ChargeHoldTime = 0.3f;

    private bool sheathed = true;
    private bool charging = false;   // button held, attack wind-up in progress
    private bool attacking = false;  // release animation in progress

    private bool Busy => animationPlayer.IsPlaying() || charging || attacking;

    public override void _Ready()
    {
        animationPlayer.SpeedScale = 1.5f;
        animationPlayer.PlayBackwards("Sheathe");
    }

    public void PlayChargeAnimation()
    {
        if (sheathed || Busy) return;

        charging = true;
        animationPlayer.Play("Attack");
    }

    public async void PlayReleaseAnimation()
    {
        if (!charging) return;

        charging = false;
        attacking = true;

        // Resumes from the paused position (or continues if not yet paused)
        animationPlayer.Play("Attack");
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

        animationPlayer.Play("RESET");
        attacking = false;
    }

    public override void _Process(double delta)
    {
        // Hold the wind-up pose once the charge point is reached
        if (charging
            && animationPlayer.IsPlaying()
            && animationPlayer.CurrentAnimation == "Attack"
            && animationPlayer.CurrentAnimationPosition >= ChargeHoldTime)
        {
            animationPlayer.Pause();
        }

        if (Input.IsActionJustPressed("sheathe") && !Busy)
        {
            sheathed = !sheathed;
            if (sheathed)
                animationPlayer.PlayBackwards("Sheathe");
            else
                animationPlayer.Play("Sheathe");
        }

        if (Input.IsActionJustPressed("attack"))
        {
            PlayChargeAnimation();
        }
        if (Input.IsActionJustReleased("attack"))
        {
            PlayReleaseAnimation();
        }
    }
}