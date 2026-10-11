using Godot;
using System;
using System.Collections;

public partial class FPSUI : CanvasLayer
{
	private Panel portrait;
	private Panel border;
	private Panel healthBar;
	private Vector2 healthBarOriginalScale;
	private Panel manaBar;
	private Vector2 manaBarOriginalScale;
	private Panel experienceBar;
	private Vector2 experienceBarOriginalScale;
	private StyleBoxTexture portraitStyleBox;
	private StyleBoxTexture borderStyleBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		portrait = GetNode<Panel>("Border/Portrait");
		border = GetNode<Panel>("Border");
		healthBar = GetNode<Panel>("Health");
		manaBar = GetNode<Panel>("Mana");
		experienceBar = GetNode<Panel>("experience");

		healthBarOriginalScale = healthBar.Scale;
		manaBarOriginalScale = manaBar.Scale;
		experienceBarOriginalScale = experienceBar.Scale;

		portraitStyleBox = portrait.GetThemeStylebox("panel") as StyleBoxTexture;
		borderStyleBox = border.GetThemeStylebox("panel") as StyleBoxTexture;

		portraitStyleBox.Texture = GlobalScript.Instance.PlayerPortrait;
		// borderStyleBox.Texture = GlobalScript.Instance.PlayerPortraitBorder;
		// adjustBorder(GlobalScript.Instance.PlayerClass);
	}

	private void adjustBorder(string playerClass)
	{
		switch (playerClass)
		{
			case "Soldier":
				border.Scale = new Vector2(6.965f, 7.305f);
				border.Position = new Vector2(119, 929);
				break;
			case "Rogue":
				border.Scale = new Vector2(4.24f, 5.905f);
				border.Position = new Vector2(121, 939);
				break;
			case "Sorcerer":
				border.Scale = new Vector2(5.805f, 7.305f);
				border.Position = new Vector2(119, 929);
				portrait.Scale = new Vector2(4.344f, 6.284f);
				portrait.Position = new Vector2(51, 865);
				break;
		}
	}

	public override void _Process(double delta)
	{
		float remainingHealthRatio = GlobalScript.Instance.PlayerHealth / GlobalScript.Instance.PlayerMaxHealth;
		float reamainingManaRatio = GlobalScript.Instance.PlayerMana / GlobalScript.Instance.PlayerMaxMana;
		float experienceRatio = GlobalScript.Instance.PlayerXP / GlobalScript.Instance.NextXPGoal;

		healthBar.Scale = new Vector2(healthBarOriginalScale[0] * remainingHealthRatio, healthBarOriginalScale[1]);
		manaBar.Scale = new Vector2(manaBarOriginalScale[0] * reamainingManaRatio, manaBarOriginalScale[1]);
		experienceBar.Scale = new Vector2(experienceBarOriginalScale[0] * experienceRatio, experienceBarOriginalScale[1]);
	}
}
