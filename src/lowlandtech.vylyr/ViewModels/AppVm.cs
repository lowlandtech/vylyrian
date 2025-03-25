using Microsoft.AspNetCore.Components;

namespace LowlandTech.Vylyr.ViewModels;

/// <summary>
/// Application-wide state manager. Controls layout state, theme, and navigation stack.
/// Designed to be platform-agnostic across MAUI, Photino, and Blazor Server/Web.
/// </summary>
public class AppVm(NavigationManager navigationManager)
{
    // -------------------------------
    // Theme & UI State
    // -------------------------------

    /// <summary>
    /// True if the side drawer is currently open.
    /// </summary>
    public bool DrawerOpen { get; set; }

    /// <summary>
    /// True if the app is in dark mode.
    /// </summary>
    public bool IsDarkMode { get; private set; } = true;

    /// <summary>
    /// Current UI theme used by MudBlazor.
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

    /// <summary>
    /// Occurs when the layout or theme state changes (used to trigger UI updates).
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Toggle drawer open/close state.
    /// </summary>
    public void ToggleDrawer()
    {
        DrawerOpen = !DrawerOpen;
        OnChange?.Invoke();
    }

    /// <summary>
    /// Explicitly close the drawer.
    /// </summary>
    public void CloseDrawer()
    {
        if (DrawerOpen)
        {
            DrawerOpen = false;
            OnChange?.Invoke();
        }
    }

    /// <summary>
    /// Toggle between light and dark themes.
    /// </summary>
    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        OnChange?.Invoke();
    }

    // -------------------------------
    // Navigation State
    // -------------------------------

    private readonly List<GraphNode> _navStack = [];
    private GraphNode? _user;

    /// <summary>
    /// Gets the current navigation stack (top node is current).
    /// </summary>
    public IReadOnlyList<GraphNode> NavStack => _navStack;

    /// <summary>
    /// The currently focused menu node (top of stack).
    /// </summary>
    public GraphNode? CurrentNode => _navStack.LastOrDefault();

    /// <summary>
    /// True if the nav stack has more than one item (i.e. can go back).
    /// </summary>
    public bool HasBack => _navStack.Count > 1;

    /// <summary>
    /// Indicates if the current *platform* is mobile (MAUI native check).
    /// </summary>
    public bool IsNativeMobile { get; set; }

    /// <summary>
    /// Indicates if the UI should behave in a mobile layout (web or native).
    /// </summary>
    public bool IsMobile { get; set; }


    /// <summary>
    /// Occurs when the navigation stack changes (used by SlidingNavMenu).
    /// </summary>
    public event Action? OnMenuChange;

    /// <summary>
    /// Initializes the navigation menu stack by loading the root user node.
    /// </summary>
    public async Task InitMenuAsync(GraphContext db)
    {
        if (_user is not null) return;

        _user = await db.Nodes
            .Include(n => n.Type)
            .FirstOrDefaultAsync(n => n.TypeId == "users");

        if (_user is not null)
        {
            _navStack.Clear();
            _navStack.Add(_user);
            NotifyMenuChanged();
        }
    }

    /// <summary>
    /// Pushes a new node onto the menu stack.
    /// </summary>
    public void PushNode(GraphNode? node)
    {
        if (node is null) return;
        _navStack.Add(node);
        NotifyMenuChanged();
    }

    /// <summary>
    /// Pops one level off the navigation stack.
    /// </summary>
    public void PopNode()
    {
        if (_navStack.Count <= 1) return;
        _navStack.RemoveAt(_navStack.Count - 1);
        NotifyMenuChanged();
    }

    /// <summary>
    /// Resets the menu stack to the root user node.
    /// </summary>
    public void ResetMenu()
    {
        _navStack.Clear();
        if (_user is null) return;
        _navStack.Add(_user);
        NotifyMenuChanged();
    }

    private void NotifyMenuChanged() => OnMenuChange?.Invoke();

    /// <summary>
    /// Gets the icon for the dark/light mode button based on the current theme.
    /// </summary>
    public string DarkLightModeButtonIcon => IsDarkMode
        ? Icons.Material.Rounded.AutoMode
        : Icons.Material.Outlined.DarkMode;

    private GraphNode? _activeNode;
    /// <summary>
    /// The currently selected node to display a component for (e.g. clicked leaf).
    /// </summary>
    public GraphNode? ActiveNode => _activeNode;

    /// <summary>
    /// Occurs when a component-viewing node is selected.
    /// </summary>
    public event Action? OnActiveNodeChanged;

    /// <summary>
    /// Sets the active node to display in the main content.
    /// </summary>
    public void SetActiveNode(GraphNode? node)
    {
        if (node is null || node.Type?.ComponentName is null)
            return;

        _activeNode = node;

        if (_activeNode.Type.Id == "page")
        {
            navigationManager.NavigateTo(_activeNode.Id);
        }
        else
        {
            navigationManager.NavigateTo("");
        }

        if (IsMobile)
            CloseDrawer();

        OnActiveNodeChanged?.Invoke();
    }
}
