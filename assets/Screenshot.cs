using Godot;

/// <summary>
/// 	Singleton to take screenshots of the game. Used to produce images at native resolution easily
/// </summary>
public partial class Screenshot : Node
{
	private int i = 0;

    public override void _UnhandledInput(InputEvent @event)
    {
		// Saves a screenshot when the "screenshot" action is detected (F12 by default)
		// Waits until the frame has been drawn to make sure everything is already rendered
        if(@event.IsActionPressed("screenshot"))
		{
			RenderingServer.FramePostDraw += SaveScreenshot;
		}
    }

	/// <summary>
	/// 	Gets the texture of the main viewport and save it to a file
	/// </summary>
	private void SaveScreenshot()
	{
		RenderingServer.FramePostDraw -= SaveScreenshot;
		GetViewport().GetTexture().GetImage().SavePng($"user://screenshot_{i++}.png");
		GD.Print($"Screenshot {i} saved");
	}
}
