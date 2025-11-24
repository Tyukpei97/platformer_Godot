using Godot;
using System;

public partial class Interactable : Area2D
{
	[Signal]
	public delegate void InteractedEventHandler();

	[Export] public string HintText = "Нажмите E чтобы открыть";
	[Export] public bool OneTime = true;

	private bool _playerInRange = false;
	private bool _alreadyUsed = false;
	private Label _hint;

	public override void _Ready()
	{
		BodyEntered += (body) => { if (body.IsInGroup("player")) { _playerInRange = true; ShowHint(true); } };
		BodyExited += (body) => { if (body.IsInGroup("player")) { _playerInRange = false; ShowHint(false); } };

		CreateHint();
	}

	public override void _Input(InputEvent @event)
	{
		if (!_playerInRange || _alreadyUsed) return;

		if (@event.IsActionPressed("interact"))
		{
			EmitSignal(SignalName.Interacted);
			if (OneTime) _alreadyUsed = true;
		}
	}

	private void CreateHint()
	{
		_hint = new Label();
		_hint.Text = HintText;
		_hint.HorizontalAlignment = HorizontalAlignment.Center;
		_hint.AddThemeFontSizeOverride("font_size", 20);
		_hint.AddThemeColorOverride("font_color", Colors.White);
		_hint.AddThemeColorOverride("font_outline_color", Colors.Black);
		_hint.AddThemeConstantOverride("outline_size", 10);
		_hint.Position = new Vector2(0, -80);
		_hint.Visible = false;
		AddChild(_hint);
	}

	private void ShowHint(bool show)
	{
		if (_hint != null) _hint.Visible = show;
	}
}
