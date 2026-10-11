using Godot;
using System;
using System.Threading.Tasks;

public partial class PauseMenu : PanelContainer
{
	private AnimationPlayer animationPlayer;
	private Control root;
	public override void _Ready()
	{
		RefocusMouseToGame();

		root = GetParent<Control>();
		root.Hide();
		
		animationPlayer = GetNode<AnimationPlayer>("../AnimationPlayer");
		animationPlayer.Play("RESET");
	}

	public override void _Process(double delta)
	{
		testEsc();
	}
	public void resume()
	{
		GetTree().Paused = false;
		animationPlayer.PlayBackwards("blur");
		root.Hide();
		RefocusMouseToGame();
	}

	public void OnResumePressed()
	{
		resume();
	}

	public void onExitPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}

	public void pause()
	{
		root.Show();
		GetTree().Paused = true;
		animationPlayer.Play("blur");
	}

	public void testEsc()
	{
		if (!GlobalScript.Instance.InInventory)
		{
			if (Input.IsActionJustPressed("esc") && !GetTree().Paused) pause();
			else if (Input.IsActionJustPressed("esc") && GetTree().Paused) resume();
		}
	}
	
    private void RefocusMouseToGame()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
