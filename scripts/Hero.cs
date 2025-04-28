using Godot;

public partial class Hero : RigidBody2D
{
    [Export] public float MoveSpeed = 200f;
    [Export] public float JumpSpeed = 200f;
    [Export] public ShapeCast2D ShapeCast;

    private bool _canJump = false;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = LinearVelocity;

        float direction = 0f;
        if (Input.IsActionPressed("ui_left"))
        {
            direction -= 1f;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            direction += 1f;
        }

        velocity.X = direction * MoveSpeed;
        ShapeCast.ForceShapecastUpdate();
        _canJump = ShapeCast.IsColliding();

        if (Input.IsActionJustPressed("ui_accept") && _canJump)
        {
            for (int i = 0; i < ShapeCast.GetCollisionCount(); i++)
            {
                Node2D collider = (Node2D)ShapeCast.GetCollider(i);
                velocity.Y = -JumpSpeed;
            }
            
        }
       
        LinearVelocity = velocity;

        if (ShapeCast.IsColliding())
            GD.Print("На земле");
        else
            GD.Print("В воздухе");

    }

}




