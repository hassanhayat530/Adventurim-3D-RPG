using Godot;
using System;
using System.Collections;

public partial class FPSUI : CanvasLayer
{
	private Panel _portrait;
	private Panel _border;
	private Panel _healthBar;
	private Vector2 _healthBarOriginalScale;
	private Panel _manaBar;
	private Vector2 _manaBarOriginalScale;
	private Panel _experienceBar;
	private Vector2 _experienceBarOriginalScale;
	private StyleBoxTexture _portraitStyleBox;
	private StyleBoxTexture _borderStyleBox;

	public override void _Ready()
	{
		_portrait = GetNode<Panel>("Border/Portrait");
		_border = GetNode<Panel>("Border");
		_healthBar = GetNode<Panel>("Health");
		_manaBar = GetNode<Panel>("Mana");
		_experienceBar = GetNode<Panel>("experience");

		_healthBarOriginalScale = _healthBar.Scale;
		_manaBarOriginalScale = _manaBar.Scale;
		_experienceBarOriginalScale = _experienceBar.Scale;

		_portraitStyleBox = _portrait.GetThemeStylebox("panel") as StyleBoxTexture;
		_borderStyleBox = _border.GetThemeStylebox("panel") as StyleBoxTexture;

		_portraitStyleBox.Texture = GlobalScript.Instance.PlayerPortrait;
		// borderStyleBox.Texture = GlobalScript.Instance.PlayerPortraitBorder;
		// adjustBorder(GlobalScript.Instance.PlayerClass);
	}

	private void adjustBorder(string playerClass)
	{
		switch (playerClass)
		{
			case "Soldier":
				_border.Scale = new Vector2(6.965f, 7.305f);
				_border.Position = new Vector2(119, 929);
				break;
			case "Rogue":
				_border.Scale = new Vector2(4.24f, 5.905f);
				_border.Position = new Vector2(121, 939);
				break;
			case "Sorcerer":
				_border.Scale = new Vector2(5.805f, 7.305f);
				_border.Position = new Vector2(119, 929);
				_portrait.Scale = new Vector2(4.344f, 6.284f);
				_portrait.Position = new Vector2(51, 865);
				break;
		}
	}

	public override void _Process(double delta)
	{
		float remainingHealthRatio = GlobalScript.Instance.PlayerHealth / GlobalScript.Instance.PlayerMaxHealth;
		float reamainingManaRatio = GlobalScript.Instance.PlayerMana / GlobalScript.Instance.PlayerMaxMana;
		float experienceRatio = GlobalScript.Instance.PlayerXP / GlobalScript.Instance.NextXPGoal;

		_healthBar.Scale = new Vector2(_healthBarOriginalScale[0] * remainingHealthRatio, _healthBarOriginalScale[1]);
		_manaBar.Scale = new Vector2(_manaBarOriginalScale[0] * reamainingManaRatio, _manaBarOriginalScale[1]);
		_experienceBar.Scale = new Vector2(_experienceBarOriginalScale[0] * experienceRatio, _experienceBarOriginalScale[1]);
	}
}
