namespace LowlandTech.Vylyr.Core.ViewModels;

public class AppVm
{
    public bool DrawerOpen { get; set; } = false;
    public bool IsDarkMode { get; private set; } = true;

    public MudTheme Theme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Black = "#110e2d",
            AppbarText = "#424242",
            AppbarBackground = "rgba(255,255,255,0.8)",
            DrawerBackground = "#ffffff",
            GrayLight = "#e8e8e8",
            GrayLighter = "#f9f9f9",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#7e6fff",
            Surface = "#1e1e2d",
            Background = "#1a1a27",
            BackgroundGray = "#151521",
            AppbarText = "#92929f",
            AppbarBackground = "rgba(26,26,39,0.8)",
            DrawerBackground = "#1a1a27",
            ActionDefault = "#74718e",
            ActionDisabled = "#9999994d",
            ActionDisabledBackground = "#605f6d4d",
            TextPrimary = "#b2b0bf",
            TextSecondary = "#92929f",
            TextDisabled = "#ffffff33",
            DrawerIcon = "#92929f",
            DrawerText = "#92929f",
            GrayLight = "#2a2833",
            GrayLighter = "#1e1e2d",
            Info = "#4a86ff",
            Success = "#3dcb6c",
            Warning = "#ffb545",
            Error = "#ff3f5f",
            LinesDefault = "#33323e",
            TableLines = "#33323e",
            Divider = "#292838",
            OverlayLight = "#1e1e2d80",
        },
    };

    public event Action? OnChange;

    public void ToggleDrawer()
    {
        DrawerOpen = !DrawerOpen;
        NotifyStateChanged();
    }

    public void CloseDrawer()
    {
        DrawerOpen = false;
        NotifyStateChanged();
    }

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private readonly List<GraphNode> _navStack = [];
    private GraphNode? _user;

    public IReadOnlyList<GraphNode> NavStack => _navStack;
    public GraphNode? CurrentNode => _navStack.LastOrDefault();
    public bool HasBack => _navStack.Count > 1;

    public event Action? OnMenuChange;

    public async Task InitMenuAsync(GraphContext db)
    {
        if (_user is not null) return;

        _user = await db.Nodes.FirstOrDefaultAsync(n => n.Type == "user");

        if (_user is not null)
        {
            _navStack.Clear();
            _navStack.Add(_user);
            OnMenuChange?.Invoke();
        }
    }

    public void PushNode(GraphNode? node)
    {
        if (node is null) return;
        _navStack.Add(node);
        OnMenuChange?.Invoke();
    }

    public void PopNode()
    {
        if (_navStack.Count <= 1) return;
        _navStack.RemoveAt(_navStack.Count - 1);
        OnMenuChange?.Invoke();
    }

    public void ResetMenu()
    {
        _navStack.Clear();
        if (_user is not null)
            _navStack.Add(_user);

        OnMenuChange?.Invoke();
    }

}
