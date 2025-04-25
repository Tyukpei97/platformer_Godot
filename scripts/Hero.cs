using Godot;

public partial class Hero : RigidBody2D
{
    [Export] public float MoveSpeed = 200f;
    [Export] public float JumpSpeed = 200f;
    [Export] public NodePath GroundShapePath;
    [Export] public bool CanJump = false;


    private ShapeCast2D _groundShape;
    private bool _canJump = false;


    public override void _Ready()
    {
        _groundShape = GetNode<ShapeCast2D>(GroundShapePath);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = LinearVelocity;

        float input = Input.GetActionRawStrength("ui_right") - Input.GetActionRawStrength("ui_left");
        velocity.X = input * MoveSpeed;

        _canJump = _groundShape.IsColliding();

        if (_canJump && Input.IsActionJustPressed("ui_accept"))
        {
            velocity.Y = -JumpSpeed;
        }

        LinearVelocity = velocity;
    }


}
