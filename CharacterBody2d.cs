using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
    [Export] private float moveSpeed ;
    [Export] private float crouchMoveSpeed;
    [Export] private float jumpSpeed;
    [Export] private float groundedTimer;
    [Export] private float groundedDelay;
    

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
        {
            velocity.Y += gravity * (float)delta;
            groundedTimer = 0f;
        }
        else
            groundedTimer += (float)delta;

        velocity.X = 0;

        if (Input.IsKeyPressed(Key.A))
        {
            velocity.X = -moveSpeed;
            if (Input.IsKeyPressed(Key.Ctrl) && IsOnFloor())
                velocity.X = -crouchMoveSpeed;
        }
        else if (Input.IsKeyPressed(Key.D))
        {
            velocity.X = moveSpeed;
            if (Input.IsKeyPressed(Key.Ctrl) && IsOnFloor())
                velocity.X = crouchMoveSpeed;
        }
        if (Input.IsKeyPressed(Key.Space) && IsOnFloor() && groundedTimer >= groundedDelay && !Input.IsKeyPressed(Key.Ctrl))
        {
            velocity.Y = -jumpSpeed;
            groundedTimer = 0f;
        }
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
        bool rolling = Input.IsKeyPressed(Key.Shift);

       var shape = (CapsuleShape2D)_collisionShape.Shape;
        if (crouching || rolling)
        {
            shape.Height = 25; 
            _collisionShape.Position = new Vector2(_collisionShape.Position.X, -15.5f);
        }
        else
        {
            shape.Height = 35;
            _collisionShape.Position = new Vector2(_collisionShape.Position.X, -20); 
        }
        

        string animation = "Idle";

        if (crouching && !jumping && !falling || rolling)
        {
            animation = walking ? "Crouch_Walk" : "Crouch";
            if (animation == walking)
                animation = "Roll";
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

