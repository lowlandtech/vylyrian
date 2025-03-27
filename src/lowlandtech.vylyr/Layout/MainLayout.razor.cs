namespace LowlandTech.Vylyr.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private AppVm App { get; set; } = default!;
    [Inject] private IJSRuntime Js { get; set; } = default!;

    private string DrawerIcon => App.DrawerOpen ? Icons.Material.Filled.Close : Icons.Material.Filled.Menu;

    protected override void OnInitialized()
    {
        App.InitializeJs(Js);
        App.OnChange += () => InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        App.OnChange -= () => InvokeAsync(StateHasChanged);
    }

    public Task HandleBreakpointChanged(Breakpoint breakpoint)
    {
        if (App.IsNativeMobile) return Task.CompletedTask;

        App.IsMobile = breakpoint <= Breakpoint.Sm;
        App.DrawerOpen = !App.IsMobile;
        StateHasChanged();
        return Task.CompletedTask;
    }

}
