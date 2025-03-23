namespace LowlandTech.Vylyr.Core.ViewModels;

/// <summary>
/// Manages global application state such as theme, drawer visibility, and navigational menu stack.
/// </summary>
public class AppVm
{
    /// <summary>
    /// Occurs when general application state changes (e.g. theme, drawer).
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Occurs when the navigation menu stack changes.
    /// </summary>
    public event Action? OnMenuChange;

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
        }
    };

    private bool _drawerOpen = false;
    private bool _isDarkMode = true;

    /// <summary>
    /// Gets or sets whether the side drawer is open.
    /// </summary>
    public bool DrawerOpen
    {
        get => _drawerOpen;
        set
        {
            if (_drawerOpen != value)
            {
                _drawerOpen = value;
                NotifyStateChanged();
            }
        }
    }

    /// <summary>
    /// Gets whether the app is in dark mode.
    /// </summary>
    public bool IsDarkMode => _isDarkMode;

    /// <summary>
    /// Toggles the side drawer open/close.
    /// </summary>
    public void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
        NotifyStateChanged();
    }

    /// <summary>
    /// Closes the side drawer.
    /// </summary>
    public void CloseDrawer()
    {
        if (_drawerOpen)
        {
            _drawerOpen = false;
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// Toggles between light and dark themes.
    /// </summary>
    public void ToggleDarkMode()
    {
        _isDarkMode = !_isDarkMode;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private readonly List<GraphNode> _navStack = [];
    private GraphNode? _user;

    /// <summary>
    /// Gets the current navigation stack.
    /// </summary>
    public IReadOnlyList<GraphNode> NavStack => _navStack;

    /// <summary>
    /// Gets the currently selected node.
    /// </summary>
    public GraphNode? CurrentNode => _navStack.LastOrDefault();

    /// <summary>
    /// Gets a value indicating whether the navigation stack has a previous item (i.e., can go back).
    /// </summary>
    public bool HasBack => _navStack.Count > 1;

    /// <summary>
    /// Initializes the navigation menu by loading the root user node from the database.
    /// </summary>
    public async Task InitMenuAsync(GraphContext db)
    {
        if (_user is not null) return;

        _user = await db.Nodes.FirstOrDefaultAsync(n => n.Type == "user");

        if (_user is not null)
        {
            _navStack.Clear();
            _navStack.Add(_user);
            NotifyMenuChanged();
        }
    }

    /// <summary>
    /// Pushes a new node onto the navigation stack.
    /// </summary>
    /// <param name="node">The node to navigate into.</param>
    public void PushNode(GraphNode? node)
    {
        if (node is null) return;

        _navStack.Add(node);
        NotifyMenuChanged();
    }

    /// <summary>
    /// Pops the last node off the navigation stack.
    /// </summary>
    public void PopNode()
    {
        if (_navStack.Count > 1)
        {
            _navStack.RemoveAt(_navStack.Count - 1);
            NotifyMenuChanged();
        }
    }

    /// <summary>
    /// Resets the navigation menu to the root user node.
    /// </summary>
    public void ResetMenu()
    {
        _navStack.Clear();

        if (_user is not null)
            _navStack.Add(_user);

        NotifyMenuChanged();
    }

    private void NotifyMenuChanged() => OnMenuChange?.Invoke();
}
