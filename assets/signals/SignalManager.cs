using Godot;

namespace Backrooms.signals;
public partial class SignalManager : Node
{
    /// <summary>
    ///     Signal emitted whenever an interaction is valid (the player is close enough and facing the trigger)
    /// </summary>
    /// <param name="uiText">Text to be displayed as a hint (either "OPEN" or "CLOSE", depending on the state of the target door)</param>
	[Signal] public delegate void EnteredInteractionDistanceEventHandler(string uiText);

    /// <summary>
    ///     Signal emitted whenever an interaction WAS valid but is no more (for example, the player gets too far away, or moves the camera away from the trigger)
    /// </summary>
    [Signal] public delegate void ExitedInteractionDistanceEventHandler();

    /// <summary>
    ///     Signal emitted whenever inputs from the player should be blocked. Movement and camera controls will become locked
    /// </summary>
    [Signal] public delegate void LockPlayerInputEventHandler();

    /// <summary>
    ///     Signal emitted whenever inputs from the player can be read again. Movement and camera controls will start working again
    /// </summary>
    [Signal] public delegate void ReleasePlayerInputEventHandler();

    /// <summary>
    ///     Signal emitted whenever text should be displayed at the bottom of the screen
    /// </summary>
    /// <param name="subtitle">The text to be shown</param>
    /// <param name="duration">The duration (in seconds) the text will remain visible</param>
    [Signal] public delegate void WriteSubtitleEventHandler(string subtitle, float duration);

    /// <summary>
    ///     Signal emitted when the game ends
    /// </summary>
    [Signal] public delegate void GameEndedEventHandler();

    /// <summary>
    ///     Signal emitted when the player needs to be snapped to a determined position
    /// </summary>
    /// <param name="node">The node used to determine the GlobalTransform of the player</param>
    [Signal] public delegate void SnapPlayerEventHandler(Node3D node);
}
