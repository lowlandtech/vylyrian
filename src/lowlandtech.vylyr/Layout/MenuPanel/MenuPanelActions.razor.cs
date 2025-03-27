namespace LowlandTech.Vylyr.Layout;

public partial class MenuPanelActions
{
    [Parameter] public EventCallback OnFilterClick { get; set; }
    [Parameter] public EventCallback OnNewNodeClick { get; set; }

    private bool Expanded = false;

    private void ToggleExpand() => Expanded = !Expanded;

    private async Task HandleFilterClick()
    {
        Expanded = false;
        await OnFilterClick.InvokeAsync();
    }

    private async Task HandleNewNodeClick()
    {
        Expanded = false;
        await OnNewNodeClick.InvokeAsync();
    }
}
