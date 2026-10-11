using Godot;
using System;

public partial class MerchantUi : Control
{
	Control playerInventory;
	Control MerchantInventory;
	Control menuButtons;
	Button back;


	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;

		playerInventory = GetNode<Control>("InventoryInstance");
		MerchantInventory = GetNode<Control>("StoreInstance");
		menuButtons = GetNode<Control>("MenuButtons");
		back = GetNode<Button>("Back");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBrowsePressed()
	{
		playerInventory.Visible = true;
		MerchantInventory.Visible = true;
		menuButtons.Visible = false;
		back.Visible = true;
	}

	public void OnLeavePressed()
	{
		if (GlobalScript.Instance.isNight)
		{
			GetNode<SceneTransitionBasic>("SceneTransition").changeScene("res://Scenes/Town.tscn");
		}
		else
        {
        	GetNode<SceneTransitionBasic>("SceneTransition").changeScene("res://Scenes/TownDay.tscn");
        }
	}

	public void OnBackPressed()
	{
		playerInventory.Visible = false;
		MerchantInventory.Visible = false;
		back.Visible = false;
		menuButtons.Visible = true;
	}
}
