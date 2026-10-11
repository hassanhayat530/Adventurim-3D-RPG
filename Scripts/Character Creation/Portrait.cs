using Godot;
using System;

public partial class Portrait : Panel
{
	[Export] private ClassLabel classLabel;
	[Export] private Label sexLabel;
	[Export] private Texture2D soldierMaleTexture;
	[Export] private Texture2D soldierFemaleTexture;
	[Export] private Texture2D soldierBorderTexture;
	[Export] private Texture2D rogueMaleTexture;
	[Export] private Texture2D rogueFemaleTexture;
	[Export] private Texture2D rogueBorderTexture;
	[Export] private Texture2D sorcererMaleTexture;
	[Export] private Texture2D sorcererFemaleTexture;
	[Export] private Texture2D sorcererBorderTexture;
	private Vector2 originalScale;
	private Vector2 originalPosition;

	private StyleBoxTexture styleBox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		styleBox = GetThemeStylebox("panel") as StyleBoxTexture;
		originalScale = Scale;
		originalPosition = Position;
		PivotOffset = Size / 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (classLabel != null)
		{
			UpdatePortraitBasedOnClass(classLabel.GetCurrentClass());
		}
	}

	private void UpdatePortraitBasedOnClass(string className)
	{
		Texture2D selectedPortraitTexture = null;
		Texture2D selectedBorderTexture = null;

		switch (className)
		{
			case "Soldier":
				if (sexLabel.Text == "Male") selectedPortraitTexture = soldierMaleTexture;
				else selectedPortraitTexture = soldierFemaleTexture;
				selectedBorderTexture = soldierBorderTexture;
				// Scale = new Vector2(1.02f, 1.4f);
				break;
			case "Rogue":
				if (sexLabel.Text == "Male")
				{
					selectedPortraitTexture = rogueMaleTexture;
					Scale = originalScale;
				}
				else
				{
					selectedPortraitTexture = rogueFemaleTexture;
					// Scale = new Vector2(1.0f, 1.38f);
				}
				selectedBorderTexture = rogueBorderTexture;
				break;
			case "Sorcerer":
				if (sexLabel.Text == "Male")
				{
					selectedPortraitTexture = sorcererMaleTexture;
					// Scale = new Vector2(1.18f, 1.75f);
				}
				else
				{
					selectedPortraitTexture = sorcererFemaleTexture;
					// Scale = new Vector2(1.18f, 1.68f);
				}
				selectedBorderTexture = sorcererBorderTexture;
				break;
		}

		styleBox.Texture = selectedPortraitTexture;
		GlobalScript.Instance.PlayerPortrait = selectedPortraitTexture;
		GlobalScript.Instance.PlayerPortraitBorder = selectedBorderTexture;
	}
}
