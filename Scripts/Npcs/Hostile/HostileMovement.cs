using Godot;

public partial class HostileMovement : CharacterBody3D
{
    [Export] private NavigationAgent3D _navigationAgent;
    [Export] public float Speed = 5.0f;
    [Export] public float TurnSpeed = 8.0f;
    [Export] private Timer _repathTimer;
    [Export] public Node3D Target;

    private bool _hasTarget;

    public override void _Ready()
    {
    _repathTimer.Timeout += OnRepathTimeout;
    }

    private void OnRepathTimeout()
    {
        if (Target != null)
            SetTarget(Target.GlobalPosition);
    }

    public void SetTarget(Vector3 position)
    {
        _navigationAgent.TargetPosition = position;
        _hasTarget = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        Vector3 velocity = Velocity;

        if (_hasTarget && !_navigationAgent.IsNavigationFinished())
        {
            Vector3 dir = _navigationAgent.GetNextPathPosition() - GlobalPosition;
            dir.Y = 0;
            dir = dir.Normalized();

            velocity.X = dir.X * Speed;
            velocity.Z = dir.Z * Speed;

            if (dir != Vector3.Zero)
            {
                float targetYaw = Mathf.Atan2(dir.X, dir.Z); // assumes the model faces +Z
                float yaw = Mathf.LerpAngle(Rotation.Y, targetYaw, TurnSpeed * dt);
                Rotation = new Vector3(Rotation.X, yaw, Rotation.Z);
            }
        }
        else
        {
            _hasTarget = false;
            velocity.X = 0;
            velocity.Z = 0;
        }

        if (!IsOnFloor())
            velocity += GetGravity() * dt;

        Velocity = velocity;
        MoveAndSlide();
    }
}