using System;
using Godot;

public partial class ToolTipInteract : Control
{
    [Export] public string ControlNodeType;
    public InventoryProperties inventoryProperties;
    Tooltip tooltip;
    InvUISlot invUISlot;
    RichTextLabel richTextLabel;
    [Export] public string TooltipContent = "Default tooltip text";

    public override void _Ready()
    {
        // tooltip.Visible = false;
        tooltip = GetNode<Tooltip>("Tooltip");
        richTextLabel = GetNode<RichTextLabel>("Tooltip/RichTextLabel");

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;

        if (ControlNodeType == "Button")
        {
            richTextLabel.BbcodeEnabled = true;

            invUISlot = GetNode<InvUISlot>("../");
            inventoryProperties = GetNode<GridContainer>("../../") as InventoryProperties;

            invUISlot.Update += update;
        }
        // ProcessMode = ProcessModeEnum.WhenPaused;
    }

    public void update()
    {
        GD.Print("Update Tooltip called");
        if (invUISlot._inventorySlot.item != null)
        {
            InventoryItem item = invUISlot._inventorySlot.item;
            int itemValue;

            if (inventoryProperties.slotsType == "Merchant")
            {
                itemValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerBuyingRatio);
            }
            else
            {
                itemValue = Convert.ToInt32(item.Value * GlobalScript.Instance.PlayerSellingRatio);
            }

            TooltipContent =
            $"[font_size=3]{item.Name}\n" +
            $"Speed Debuff: {item.SpeedDebuff}%\n" +
            $"Effect: {item.Effect}\n" +
            $"Value: {itemValue} gold" +
             $"[/font_size]";
        }   
    }

    private void OnMouseEntered()
    {
        tooltip.Toggle(true, TooltipContent);
    }

    private void OnMouseExited()
    {
        tooltip.Toggle(false);
    }
}