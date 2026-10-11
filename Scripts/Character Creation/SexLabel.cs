using Godot;
using System;

public partial class SexLabel : Label
{
	private Label sex;
	private string[] sexes = ["Male", "Female"];
	private int currSexIndex = 0;

	public override void _Ready()
	{
		sex = GetNode<Label>("Sex");
	}

	private void OnRightArrowPressed()
	{
		if (currSexIndex != 1)
		{
			++currSexIndex;
			sex.Text = sexes[currSexIndex];
			GlobalScript.Instance.PlayerSex = sexes[currSexIndex];
		}
	}

	private void OnLeftArrowPressed()
	{
		if (currSexIndex != 0)
		{
			--currSexIndex;
			sex.Text = sexes[currSexIndex];
			GlobalScript.Instance.PlayerSex = sexes[currSexIndex];
		}
	}
}
