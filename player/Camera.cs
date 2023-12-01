using Backrooms.signals;
using Godot;

namespace Backrooms.player;
/// <summary>
/// 	Camera Node able to be controlled by the mouse
/// </summary>
public partial class Camera : Camera3D
{
	private Player _player;
	private SignalManager _signalManager;
	private bool _controlsLocked = true;

	public override void _Ready()
	{
		_player = (Player) Owner;
		_signalManager = GetNode<SignalManager>("/root/SignalManager");

		_signalManager.LockPlayerInput += () => _controlsLocked = true;
		_signalManager.ReleasePlayerInput += () => _controlsLocked = false;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		// If the "LockPlayerInput" global signal was emitted, the camera cannot be moved
		if(_controlsLocked)
		{
			return;
		}

		// Checks if the detected event comes from mouse movement. If it does, moves the camera
        if(@event is InputEventMouseMotion motion)
		{
			_player.RotationDegrees -= new Vector3(0f, motion.Relative.X, 0f);

			var cameraRotation = new Vector3(Mathf.Clamp(RotationDegrees.X - motion.Relative.Y, -70f, 70f), RotationDegrees.Y, 0f);

			RotationDegrees = cameraRotation;
		}
    }
}
