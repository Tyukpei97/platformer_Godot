using Godot;
using System;

public partial class DamageHadler : Node2D
{
	public int Health = 1; 
	private AnimatedSprite2D _animations;

	public void TakeDamage()
	{
		Health -= 1;
		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		_animations = GetNode<AnimatedSprite2D>("animations");
		_animations.Play("Death_NoMove");
		QueueFree();
		GD.Print($"{GetParent().Name} умер");
	}

	public bool IsDead()
	{
		return Health <= 0;
	}
}
