using Backrooms.signals;
using Godot;

namespace Backrooms.assets;
/// <summary>
/// 	Base class for all Doors in the game
/// </summary>
public partial class Door : Node3D
{
	/// <summary>
	/// 	Defines if this door is locked or not. Locked doors cannot be opened
	/// </summary>
	[Export] public bool IsLocked { get; private set; } = false;

	/// <summary>
	/// 	Defines if this door is open or closed. Interacting with a door will flip this variable
	/// </summary>
	public bool IsOpen { get; private set;} = false;

	private protected SignalManager _signalManager;

    public override void _Ready()
    {
        _signalManager = GetNode<SignalManager>("/root/SignalManager");
    }

	/// <summary>
	/// 	Virtual method used to interact with this door. Can be overriden by children.
	/// 	By default, checks if the door is locked or not. If not, the door will be opened/closed, depending on it's previous state
	/// </summary>
    public virtual void Interact()
	{
		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		// If the door is locked, plays the "locked" animation, displays the text on screen, and returns
		if(IsLocked)
		{
			animationPlayer.Play("locked");

			_signalManager.EmitSignal(SignalManager.SignalName.WriteSubtitle, Tr("LOCKED"), 1f);

			return;
		}

		// If the door is closed, open it. If it's open, close it
		switch(IsOpen)
		{
			case true:
				animationPlayer.Play("close");
				IsOpen = false;
				break;

			case false:
				animationPlayer.Play("open");
				IsOpen = true;
				break;
		}
	}
}
