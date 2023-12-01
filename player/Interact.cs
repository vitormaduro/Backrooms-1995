using Backrooms.assets;
using Backrooms.signals;
using Godot;

namespace Backrooms.player;
/// <summary>
///     Node used to control how the player can interact with objects
/// </summary>
public partial class Interact : RayCast3D
{
    private SignalManager _signalManager;
    private Door _targetDoor;

    public override void _Ready()
    {
        _signalManager = GetNode<SignalManager>("/root/SignalManager");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Checks if the detected input caused a door to enter the interaction range, or if a previous door left this range
        if(_targetDoor is null && IsColliding() && ((Node3D) GetCollider()).Owner is Door door)
        {
            _targetDoor = door;

            // A new door entered the interaction range. The interaction hint should be displayed on screen
            _signalManager.EmitSignal(SignalManager.SignalName.EnteredInteractionDistance, _targetDoor.IsOpen ? "CLOSE" : "OPEN");
        }
        else if(_targetDoor is not null && !IsColliding())
        {
            _targetDoor = null;

            // A previously detected door left the interaction range. The interaction hint should be hidden
            _signalManager.EmitSignal(SignalManager.SignalName.ExitedInteractionDistance);
        }

        // If a door is within interaction range and the "interact" action is detect, interact with it
        if(@event.IsActionPressed("interact") && _targetDoor is not null)
		{
			_targetDoor.Interact();
		}
    }
}
