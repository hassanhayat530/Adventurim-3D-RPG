using Godot;
using System;

public partial class FontScalar : Node
{
	[Export] public float DefaultFontSize { get; set; } = 35f;
	[Export] public float defaultResoltionWidth { get; set; } = 1920f;
	Control parentNode;
	public override void _Ready()
	{
		parentNode = GetParent<Control>();

		if (parentNode.GetType() == typeof(Button) || parentNode.GetType() == typeof(Label))
		{
			ScaleTextSize();
		} else
		{
			GD.PrintErr("FontScalar script should be attached to a Button or Label node.");
		}
	}

	public void ScaleTextSize()
	{
		float currentResolutionWidth = DisplayServer.WindowGetSize().X;
		float scaleFactor = currentResolutionWidth / defaultResoltionWidth;
		parentNode.AddThemeFontSizeOverride("font_size", (int)(DefaultFontSize * scaleFactor));
	}
}
