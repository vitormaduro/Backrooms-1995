using System.Threading;
using Godot;

namespace Backrooms.ui;
/// <summary>
/// 	Main menu of the game, with buttons to start and quit the game
/// </summary>
public partial class Menu : Control
{
	public override void _Ready()
	{
		// Makes sure the mouse is visible
		Input.MouseMode = Input.MouseModeEnum.Visible;

		var startButton = GetNode<Button>("%StartButton");
		var quitButton = GetNode<Button>("%QuitButton");
		var preloader = GetNode<ResourcePreloader>("ResourcePreloader");
		var culture = Thread.CurrentThread.CurrentCulture.Name;

		// Sets the locale of the game depending on the culture of the system running the game
		switch(culture)
		{
			case "pt" or "pt-BR" or "pt-PT":
				TranslationServer.SetLocale("pt");
				break;

			default:
				TranslationServer.SetLocale("en");
				break;
		}

		//	Adds ">>" to the button's text when the mouse hovers over it
		startButton.MouseEntered += () => OnMouseEntered(startButton);
		quitButton.MouseEntered += () => OnMouseEntered(quitButton);

		// Removes the ">>" from the button's text when the mouse leaves it
		startButton.MouseExited += () => OnMouseExited(startButton);
		quitButton.MouseExited += () => OnMouseExited(quitButton);

		// Start the game
		startButton.Pressed += () => GetTree().ChangeSceneToPacked((PackedScene) preloader.GetResource("level"));

		// Quit the game
		quitButton.Pressed += () => GetTree().Quit();
	}

	/// <summary>
	/// 	Changes the text of a button to ">>{text}", where {text} is the previous text of the button
	/// </summary>
	/// <param name="button">The target button</param>
	private void OnMouseEntered(Button button)
	{
		button.Text = $">>{Tr(button.Text)}";
	}

	/// <summary>
	/// 	Changes the text of a button to "{text}", removing the ">>" added by <see cref="OnMouseEntered"/>
	/// </summary>
	/// <param name="button">The target button</param>
	private static void OnMouseExited(Button button)
	{
		button.Text = button.Text.Replace(">>", "");
	}
}
