using Godot;

public partial class Door : Node2D
{
	public override void _Ready()
	{
		// Дверь по умолчанию закрыта
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("closed");
	}

	// Этот метод вызывается Interactable при нажатии E
	public void Open()
	{
		var анимация = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		анимация.Play("open");

		// Звук открывания (если узел AudioStreamPlayer2D есть в сцене)
		var звук = GetNodeOrNull<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		if (звук != null)
			звук.Play();

		GD.Print("Дверь открылась!");
	}
}
