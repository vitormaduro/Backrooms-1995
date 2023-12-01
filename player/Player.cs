using System.Collections.Generic;
using System.Linq;
using Backrooms.signals;
using Godot;

namespace Backrooms.player;
/// <summary>
/// 	Node representing the player character
/// </summary>
public partial class Player : CharacterBody3D
{
	private const float _speed = 1.0f;

	private readonly List<AudioStreamMP3> _steps = new();
	private readonly RandomNumberGenerator _rng = new();

	private bool _controlsLocked = true;
	private SignalManager _signalManager;
	private AudioStreamPlayer3D _audioPlayer;
	private float _stepDelay = 0f;

    public override void _Ready()
    {
        _signalManager = GetNode<SignalManager>("/root/SignalManager");
		_audioPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		_signalManager.LockPlayerInput += () => _controlsLocked = true;
		_signalManager.ReleasePlayerInput += () => _controlsLocked = false;
		_signalManager.SnapPlayer += (Node3D anchor) => 
		{
			GetNode<Camera3D>("Camera3D").RotationDegrees = new(0f, -180f, 0f);

			GetTree().CreateTween().TweenProperty(this, "global_transform", anchor.GlobalTransform, 0.2f);
		};

		_signalManager.GameEnded += () =>
		{
			GetNode<AudioStreamPlayer3D>("Camera3D/AudioStreamPlayer3D").Stop();
		};

		// Loads all 12 footsteps sound files in memory
		for(var i = 1; i <= 12; i++)
		{
			_steps.Add(GD.Load<AudioStreamMP3>($"res://player/steps/steps-{i}.mp3"));
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		// If the "LockPlayerInput" global event was emitted, the player cannot move
		if(_controlsLocked)
		{
			return;
		}

		var velocity = Velocity;
		var inputDir = Input.GetVector("move_right", "move_left", "move_back", "move_forward");
		var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		// Moves the player if any movement input was detected
		if(direction != Vector3.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;

			_stepDelay -= (float) delta;

			// Plays a randomly selected footstep at regular intervals
			if(_stepDelay <= 0f)
			{
				var id = _rng.RandiRange(1, _steps.Count);

				_audioPlayer.Stream = _steps.Skip(id - 1).First();
				_audioPlayer.Play();

				_stepDelay = 0.8f;
			}
		}
		else
		{
			// If no movement input was detected, stops the player and resets the footstep timer
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);

			_stepDelay = 0.3f;
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
