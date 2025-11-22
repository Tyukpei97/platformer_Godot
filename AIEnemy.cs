using Godot;
using System;

public partial class AIEnemy : CharacterBody2D
{
	private AnimatedSprite2D sprite_mob;
	private RayCast2D leftRay;
	private RayCast2D rightRay;

	private Vector2 velocity = new Vector2();
	private float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	private float speed = 100f;
	private bool movingRight = true;

	public override void _Ready()
	{
		sprite_mob = GetNode<AnimatedSprite2D>("animations_enemy");
		leftRay = GetNode<RayCast2D>("left");
		rightRay = GetNode<RayCast2D>("right");

		if (!leftRay.IsEnabled())
			leftRay.Enabled = true;
		if (!rightRay.IsEnabled())
			rightRay.Enabled = true;

		velocity.X = speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity.X = movingRight ? speed : -speed;

		// Проверка столкновений лучами
		if (movingRight && rightRay.IsColliding())
		{
			movingRight = false;
			GD.Print("Столкновение справа, меняем направление на лево");
		}
		else if (!movingRight && leftRay.IsColliding())
		{
			movingRight = true;
			GD.Print("Столкновение слева, меняем направление на право");
		}

		// Отладочный вывод
		GD.Print($"MovingRight: {movingRight}, Velocity.X: {velocity.X}");

		// Обновление анимации
		UpdateSpriteRenderer(velocity.X, velocity.Y);

		velocity.Y += gravity * (float)delta;
		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateSpriteRenderer(float velX, float velY)
	{
		bool walking = Math.Abs(velX) > 0.1f;
		string animation = walking ? "move" : "idle";

		// Отладочный вывод
		GD.Print($"VelX: {velX}, Walking: {walking}, Анимация: {animation}");

		// Воспроизведение анимации
		sprite_mob.Play(animation);

		// Отражение по горизонтали
		if (walking)
			sprite_mob.FlipH = velX > 0;
	}
}
