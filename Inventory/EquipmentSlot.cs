using Godot;
using System;

public partial class EquipmentSlot : Panel
{
	[Export] public string equipmentType { get; set; }
	[Export] public Texture2D nullTexture { get; set; }
	private Sprite2D nullVisual;
	private Sprite2D backgroundVisual;
	private Sprite2D equipVisual;

	public override void _Ready()
	{
		nullVisual = GetNode<Sprite2D>("CenterContainer/Panel/NullDisplay");
		backgroundVisual = GetNode<Sprite2D>("CenterContainer/Panel/Background");
		equipVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");

		nullVisual.Texture = nullTexture;
		backgroundVisual.Visible = false;
		equipVisual.Visible = false;
	}

	public void empty()
	{
		backgroundVisual.Visible = false;
		equipVisual.Visible = false;
	}

	public void fill(InventoryItem item)
	{
		equipVisual.Texture = item.Texture;
		backgroundVisual.Visible = true;
		equipVisual.Visible = true;
	}
}
