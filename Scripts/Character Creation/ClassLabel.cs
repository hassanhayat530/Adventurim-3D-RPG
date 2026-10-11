using Godot;
using System;
using System.Collections.Generic;

public partial class ClassLabel : Label
{
	[Export] private Label constitutionValue;
	[Export] private Label strengthValue;
	[Export] private Label dexterityValue;
	[Export] private Label intelligenceValue;
	[Export] private Label charismaValue;
	[Export] private Label speedValue;
	private string[] classes = ["Soldier", "Rogue", "Sorcerer"];
	private int currClassIndex;
	private Label classNode;
	private string[][] attributes =
		[
			["25", "25", "15", "10", "10", "15"], // soldier
			["10", "10", "25", "10", "20", "25"], // rogue
			["15", "10", "15", "25", "15", "20"]  // sorcerer
		];
	private Label[] attributeLabels;

	public override void _Ready()
	{
		classNode = GetNode<Label>("Class");
		attributeLabels = [constitutionValue, strengthValue, dexterityValue, intelligenceValue, charismaValue, speedValue];
		currClassIndex = 0;
		classNode.Text = classes[currClassIndex];
		ChangeAttributes(attributes[currClassIndex]);
	}

	private void ChangeAttributes(string[] values)
	{
		for (int i = 0; i < 6; ++i)
		{
			attributeLabels[i].Text = values[i];
		}
		GlobalScript.Instance.UpdateAttributes(values);
	}

	private void OnRightArrowPressed()
	{
		if (currClassIndex != 2)
		{
			currClassIndex += 1;
			classNode.Text = classes[currClassIndex];
			ChangeAttributes(attributes[currClassIndex]);
			GlobalScript.Instance.PlayerClass = classes[currClassIndex];
			
		}
	}

	private void OnLeftArrowPressed()
	{
		if (currClassIndex != 0)
		{
			currClassIndex -= 1;
			classNode.Text = classes[currClassIndex];
			ChangeAttributes(attributes[currClassIndex]);
			GlobalScript.Instance.PlayerClass = classes[currClassIndex];
		}
	}

	public string GetCurrentClass()
	{
		return classes[currClassIndex];
	}
}
