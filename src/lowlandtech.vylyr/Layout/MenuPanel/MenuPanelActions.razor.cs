using LowlandTech.Vylyr.Infrastructure.Enums;

namespace LowlandTech.Vylyr.Layout;

public partial class MenuPanelActions
{
    [Parameter] public EventCallback<FooterMode> OnFooterAction { get; set; }
    [Parameter] public FooterMode CurrentFooterMode { get; set; }

    private bool _expanded = false;

    private void ToggleExpand() => _expanded = !_expanded;

    private async Task HandleFilterClick()
    {
        _expanded = false;
        CurrentFooterMode = FooterMode.Filter;
        await OnFooterAction.InvokeAsync(CurrentFooterMode);
    }

    private async Task HandleNewNodeClick()
    {
        _expanded = false;
        CurrentFooterMode = FooterMode.NewNode;
        await OnFooterAction.InvokeAsync(CurrentFooterMode);
    }
}
