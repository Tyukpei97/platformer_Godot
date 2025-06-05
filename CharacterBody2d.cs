using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CharacterBody2d : CharacterBody2D
{
    [Export] private float maxMoveSpeed;
    [Export] private float crouchMoveSpeed;
    [Export] private float jumpSpeed;
    [Export] private float groundedDelay;
    [Export] private float rollSpeedBoost;
    [Export] private float rollDuration;

    private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private AnimatedSprite2D _animations;
    private CollisionShape2D _collisionShape;
    
    private float _rollTimer;
    private float _accelerationTime;
    private float _totalTime;
    private bool _isRolling;
    private float _groundedTimer;

    public override void _Ready()
    {
        _animations = GetNode<AnimatedSprite2D>("animations");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        float _currentSpeed = (_accelerationTime / _totalTime) * maxMoveSpeed;

        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            _groundedTimer = 0.0f;
        }
        else
            _groundedTimer += (float)delta;

        velocity.X = 0.0f;
        bool crouching = Input.IsKeyPressed(Key.Ctrl);
        bool rollInput = Input.IsActionJustPressed("Roll") && !_isRolling && IsOnFloor() && !crouching;

        if (_isRolling)
        {
            _rollTimer -= (float)delta;
            if (_rollTimer <= 0.0f)
                _isRolling = false;
            velocity.X = (_animations.FlipH ? -1.0f : 1.0f) * rollSpeedBoost;
        }
        else
        {
            if (Input.IsKeyPressed(Key.A))
            {
                _accelerationTime += (float)delta;
                velocity.X = crouching ? -crouchMoveSpeed : -_currentSpeed;
            }
            else if (Input.IsKeyPressed(Key.D))
            {
                _accelerationTime += (float)delta;
                velocity.X = crouching ? crouchMoveSpeed : _currentSpeed;
            }
        }

        if (Input.IsKeyPressed(Key.Space) && IsOnFloor() && _groundedTimer >= groundedDelay && !crouching && !_isRolling)
        {
            velocity.Y = -jumpSpeed;
            _groundedTimer = 0.0f;
        }

        if (rollInput)
        {
            _isRolling = true;
            _rollTimer = rollDuration;
        }

        UpdateSpriteRenderer(velocity.X, velocity.Y);
        Velocity = velocity;
        MoveAndSlide();
    }

    private void UpdateSpriteRenderer(float velX, float velY)
    {
        bool walking = Math.Abs(velX) > 0.1f;
        bool jumping = velY < -0.1f;
        bool falling = velY > 0.1f;
        bool crouching = Input.IsKeyPressed(Key.Ctrl);

        var shape = (CapsuleShape2D)_collisionShape.Shape;
        bool isLowProfile = crouching || _isRolling;
        shape.Height = isLowProfile ? 25.0f : 35.0f;
        _collisionShape.Position = new Vector2(_collisionShape.Position.X, isLowProfile ? -15.5f : -20.0f);

        string animation = _isRolling ? "Roll" :
            crouching && !jumping && !falling ? (walking ? "Crouch_Walk" : "Crouch") :
            jumping ? "Jump" :
            falling ? "Fall" :
            walking ? "Run" : "Idle";

        if (_animations.Animation != animation)
            _animations.Play(animation);

        if (walking && !_isRolling)
            _animations.FlipH = velX < 0;
    }
}