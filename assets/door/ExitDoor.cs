using Backrooms.signals;
using Godot;

namespace Backrooms.assets;
/// <summary>
/// 	The last door in the game
/// </summary>
public partial class ExitDoor : Door
{
	/// <summary>
	/// 	After being opened, triggers the dialog with the security guard. Then emits a signal to end the game
	/// </summary>
    public override async void Interact()
    {
        base.Interact();

		await ToSignal(GetTree().CreateTimer(1f), "timeout");

		_signalManager.EmitSignal(SignalManager.SignalName.LockPlayerInput);
		_signalManager.EmitSignal(SignalManager.SignalName.SnapPlayer, GetNode<Node3D>("CameraAnchor"));

		await ToSignal(GetTree().CreateTimer(2f), "timeout");

		_signalManager.EmitSignal(SignalManager.SignalName.WriteSubtitle, Tr("ENDING_1"), 3f);

		await ToSignal(GetTree().CreateTimer(3f), "timeout");

		_signalManager.EmitSignal(SignalManager.SignalName.WriteSubtitle, Tr("ENDING_2"), 3f);

		await ToSignal(GetTree().CreateTimer(3f), "timeout");

		_signalManager.EmitSignal(SignalManager.SignalName.GameEnded);
    }
}
