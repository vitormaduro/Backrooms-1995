using Backrooms.signals;

namespace Backrooms.assets;
/// <summary>
///     The first door of the game
/// </summary>
public partial class EntryDoor : Door
{
    /// <summary>
    ///     Instead of opening/closing, interacting with this door will instead display a special message
    /// </summary>
    public override void Interact()
    {
        _signalManager.EmitSignal(SignalManager.SignalName.WriteSubtitle, $"{Tr("START_1")}\n{Tr("START_2")}", 3f);
    }
}
