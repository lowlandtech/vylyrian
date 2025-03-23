namespace LowlandTech.Vylyr.Core.ViewModels;

/// <summary>
/// Application-wide ViewModel managing theme, drawer, and navigation state.
/// </summary>
public partial class AppVm : ObservableObject
{
    // ============================
    // Theme & UI State
    // ============================

    [ObservableProperty]
    private bool _drawerOpen;

    [ObservableProperty]
    private bool _isDarkMode = true;

    /// <summary>
    /// Gets the current MudBlazor theme.
    /// </summary>
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

    // ============================
    // Navigation Stack
    // ============================

    private GraphNode? _user;

    /// <summary>
    /// Navigation stack of the menu.
    /// </summary>
    public ObservableCollection<GraphNode> NavStack { get; } = new();

    /// <summary>
    /// Gets the current node (top of the stack).
    /// </summary>
    public GraphNode? CurrentNode => NavStack.LastOrDefault();

    /// <summary>
    /// Whether the nav stack has previous nodes (can go back).
    /// </summary>
    public bool HasBack => NavStack.Count > 1;

    public event Action? OnMenuChange;

    /// <summary>
    /// Initializes the navigation menu from database.
    /// </summary>
    public async Task InitMenuAsync(GraphContext db)
    {
        if (_user is not null) return;

        _user = await db.Nodes.FirstOrDefaultAsync(n => n.Type == "user");

        if (_user is not null)
        {
            NavStack.Clear();
            NavStack.Add(_user);
            OnMenuChange?.Invoke();
        }
    }

    /// <summary>
    /// Pushes a new node onto the stack.
    /// </summary>
    [RelayCommand]
    public void PushNode(GraphNode? node)
    {
        if (node is null) return;
        NavStack.Add(node);
        OnMenuChange?.Invoke();
    }

    /// <summary>
    /// Pops one level from the stack.
    /// </summary>
    [RelayCommand]
    public void PopNode()
    {
        if (NavStack.Count > 1)
        {
            NavStack.RemoveAt(NavStack.Count - 1);
            OnMenuChange?.Invoke();
        }
    }

    /// <summary>
    /// Resets the stack to the user root node.
    /// </summary>
    [RelayCommand]
    public void ResetMenu()
    {
        NavStack.Clear();
        if (_user is not null)
        {
            NavStack.Add(_user);
            OnMenuChange?.Invoke();
        }
    }

    /// <summary>
    /// Toggles the theme mode.
    /// </summary>
    [RelayCommand]
    public void ToggleDarkMode() => IsDarkMode = !IsDarkMode;

    /// <summary>
    /// Toggles the drawer.
    /// </summary>
    [RelayCommand]
    public void ToggleDrawer() => DrawerOpen = !DrawerOpen;

    /// <summary>
    /// Closes the drawer.
    /// </summary>
    [RelayCommand]
    public void CloseDrawer() => DrawerOpen = false;
}
