using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
    [Export] public float moveSpeed = 150f;
    [Export] public float jumpSpeed = 400f;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D _animations;
    private CollisionShape2D _collisionShape;
    public override void _Ready()
    {
        _animations = GetNode<AnimatedSprite2D>("animations");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        
    }


    public override void _PhysicsProcess(double delta) 
    {
        
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y += gravity * (float) delta;

        velocity.X = 0;

        if (Input.IsKeyPressed(Key.A))
            velocity.X = -moveSpeed;
        else if (Input.IsKeyPressed(Key.D))
            velocity.X = moveSpeed;

        if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
            velocity.Y = -jumpSpeed;

        _UpdateSpriteRenderer(velocity.X, velocity.Y);
        Velocity = velocity;
        MoveAndSlide();
    }

    private void _UpdateSpriteRenderer(float velX, float velY)
    {
        bool walking = Math.Abs(velX) > 0.1f;
        bool jumping = velY < -0.1f;
        bool falling = velY > 0.1f;
        bool crouching = Input.IsKeyPressed(Key.Ctrl);

       var shape = (CapsuleShape2D)_collisionShape.Shape;
        if (crouching)
        {
            shape.Height = 20; 
        }
        else
        {
            shape.Height = 32; 
        }

        string animation = "Idle";

        if (crouching)
        {
            animation = walking ? "Crouch_Walk" : "Crouch";
        }
        else
        {
            if (jumping)
                animation = "Jump";
            else if (falling)
                animation = "Fall";
            else if (walking)
                animation = "Run";
            else
                animation = "Idle";
        }

        if (_animations.Animation != animation)
        {
            _animations.Play(animation);
        }

        if (Math.Abs(velX) > 0.1f)
        {
            _animations.FlipH = velX < 0;
        }
    }
}

