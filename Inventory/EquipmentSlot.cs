using Godot;
using System;

public partial class EquipmentSlot : Panel
{
	[Export] public string EquipmentType { get; set; }
	[Export] public Texture2D NullTexture { get; set; }
	private Sprite2D _nullVisual;
	private Sprite2D _backgroundVisual;
	private Sprite2D _equipVisual;

	public override void _Ready()
	{
		_nullVisual = GetNode<Sprite2D>("CenterContainer/Panel/NullDisplay");
		_backgroundVisual = GetNode<Sprite2D>("CenterContainer/Panel/Background");
		_equipVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");

		_nullVisual.Texture = NullTexture;
		_backgroundVisual.Visible = false;
		_equipVisual.Visible = false;
	}

	public void Empty()
	{
		_backgroundVisual.Visible = false;
		_equipVisual.Visible = false;
	}

	public void Fill(InventoryItem item)
	{
		_equipVisual.Texture = item.Texture;
		_backgroundVisual.Visible = true;
		_equipVisual.Visible = true;
	}
}
