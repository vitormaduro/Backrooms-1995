using System;
using Backrooms.signals;
using Godot;

namespace Backrooms.ui;
/// <summary>
/// 	Node responsible for the UI (overlays, camcorder info, etc.)
/// </summary>
public partial class UI : Control
{
	private Label _dateTimeLabel;
	private Label _recTime;
	private TextureRect _recIcon;
	private Label _interactLabel;
	private Label _subtitleLabel;
	private DateTime _timer = new(1995, 11, 06, 00, 15, 41);
	private Color _normalColor = new(1, 1, 1, 1);
	private Color _transparentColor = new(1, 1, 1, 0);
	private SignalManager _signalManager;
	private AudioStreamPlayer _audioPlayer;

	public override void _Ready()
	{
		var fade = GetNode<ColorRect>("Fade");

		fade.Show();

		GetNode<CanvasLayer>("EndingCutscene").Hide();

		_dateTimeLabel = GetNode<Label>("%DateTime");
		_recTime = GetNode<Label>("%RecTime");
		_recIcon = GetNode<TextureRect>("%RecIcon");
		_interactLabel = GetNode<Label>("%InteractLabel");
		_subtitleLabel = GetNode<Label>("%Subtitles");
		_signalManager = GetNode<SignalManager>("/root/SignalManager");
		_audioPlayer = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

		_interactLabel.Hide();
		_subtitleLabel.Hide();

		_signalManager.EnteredInteractionDistance += (string uiText) =>
		{
			_interactLabel.Show();
			_interactLabel.Text = "[E]\n" + _interactLabel.Tr(uiText);
		};

		_signalManager.ExitedInteractionDistance += () =>
		{
			_interactLabel.Hide();
		};

		_signalManager.WriteSubtitle += DisplaySubtitle;
		_signalManager.GameEnded += EndGame;

		// When the game starts, waits for 2 seconds with a black screen
		// Then tween the alpha to 0 in 5 seconds
		GetTree().CreateTimer(2f).Timeout += () =>
		{
			var tween = GetTree().CreateTween();

			tween.TweenProperty(fade, "color", new Color(0f, 0f, 0f, 0f), 5f);
			tween.Finished += () => _signalManager.EmitSignal(SignalManager.SignalName.ReleasePlayerInput);
		};
	}

	public override void _Process(double delta)
	{
		// Increases the playing time, the clock, and blink the Recording icon
		_timer = _timer.AddSeconds(delta);
		_dateTimeLabel.Text = $"1995-11-06\n{DateTime.Now:HH:mm:ss}";
		_recTime.Text = _timer.ToString("HH:mm:ss.fff");
		_recIcon.Modulate = _timer.Second % 2 == 0 ? _normalColor : _transparentColor;
	}

	/// <summary>
	/// 	Displays text on the bottom of the screen, with the correct font and a black box behind it
	/// </summary>
	/// <param name="subtitle">The text to be displayed</param>
	/// <param name="duration">The duration (in seconds) that this text will remain visible</param>
	private void DisplaySubtitle(string subtitle, float duration)
	{
		_signalManager.EmitSignal(SignalManager.SignalName.LockPlayerInput);

		_subtitleLabel.Show();
		_subtitleLabel.Text = subtitle;

		GetTree().CreateTimer(duration).Timeout += () => 
		{
			_subtitleLabel.Hide();
			_signalManager.EmitSignal(SignalManager.SignalName.ReleasePlayerInput);
		};
	}

	/// <summary>
	/// 	Ends the game, showing the final screen, playing the correct SFX, and closing the game
	/// </summary>
	private async void EndGame()
	{
		_signalManager.EmitSignal(SignalManager.SignalName.LockPlayerInput);
		_audioPlayer.Stream = GD.Load<AudioStreamMP3>("res://player/camcorder.mp3");
		_audioPlayer.Play();

		GetNode<CanvasLayer>("EndingCutscene").Show();

		await ToSignal(GetTree().CreateTimer(3f), "timeout");

		_audioPlayer.Stream = GD.Load<AudioStreamMP3>("res://assets/ui/tape-end.mp3");
		_audioPlayer.Play();

		await ToSignal(_audioPlayer, "finished");
		await ToSignal(GetTree().CreateTimer(2f), "timeout");

		GetTree().Quit();
	}
}
