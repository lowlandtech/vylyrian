namespace LowlandTech.Vylyr.Layout;

public partial class MenuPanel
{
    private const string CardClasses = "footer-card pa-2 mb-2";

    [Parameter] public GraphNode CurrentNode { get; set; } = default!;
    [Parameter] public EventCallback<GraphNode> OnNavigate { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }
    [Parameter] public bool ShowBack { get; set; } = true;

    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private GraphContext Db { get; set; } = default!;
    [Inject] private IJSRuntime Js { get; set; } = default!;
    [Inject] private AppVm App { get; set; } = default!;

    private List<GraphNodeType> _availableTypes = [];
    private List<GraphNodeWithCount>? _children;

    private GraphNode _newNode = new();

    [Parameter] public FooterMode CurrentFooterMode { get; set; }
    [Parameter] public EventCallback OnResetFooter { get; set; }

    private List<GraphNodeWithCount> FilteredChildren =>
        string.IsNullOrWhiteSpace(_newNode.Title)
            ? _children ?? []
            : _children?.Where(c =>
                  c.Title.Contains(_newNode.Title, StringComparison.OrdinalIgnoreCase)).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        _availableTypes = await Db.NodeTypes.OrderBy(t => t.Label).ToListAsync();
        var defaultType = _availableTypes.FirstOrDefault(t => t.Id == "list") ?? _availableTypes.First();

        _newNode = new GraphNode
        {
            Title = "",
            Type = defaultType
        };

        await LoadChildrenAsync();
    }

    private async Task LoadChildrenAsync()
    {
        _children = await Db.Edges
            .Where(e => e.FromId == CurrentNode.Id)
            .Select(e => e.To)
            .Select(n => new GraphNodeWithCount
            {
                Id = n.Id,
                Title = n.Title,
                Type = n.Type,
                Route = n.Route,
                ChildCount = Db.Edges.Count(e => e.FromId == n.Id)
            })
            .ToListAsync();
    }

    private async Task TryNavigate(GraphNodeWithCount node)
    {
        if (node.ChildCount > 0)
        {
            await OnNavigate.InvokeAsync(node);
        }
        else
        {
            App.SetActiveNode(node);
        }
    }

    private string GetListItemStyle(GraphNodeWithCount node)
    {
        var isActive = App.ActiveNode?.Id == node.Id;
        var textColor = App.IsDarkMode
            ? App.Theme.PaletteDark.TextPrimary
            : App.Theme.PaletteLight.AppbarText;

        var background = isActive ? App.IsDarkMode ? "#2a2833" : "#f0f0f0" : "transparent";
        return $"color: {textColor}; background-color: {background}; transition: background-color 0.2s;";
    }

    private async Task CreateNewNode()
    {
        if (string.IsNullOrWhiteSpace(_newNode.Title))
            return;

        var newNode = new GraphNode
        {
            Id = Guid.NewGuid().ToString(),
            Title = _newNode.Title,
            Type = _newNode.Type
        };

        Db.Nodes.Add(newNode);
        Db.Edges.Add(new GraphEdge { FromId = CurrentNode.Id, ToId = newNode.Id });

        await Db.SaveChangesAsync();

        _children ??= new();
        _children.Add(new GraphNodeWithCount
        {
            Id = newNode.Id,
            Title = newNode.Title,
            Type = newNode.Type,
            ChildCount = 0
        });

        // Reset the model to refresh the form
        _newNode = new GraphNode
        {
            Title = string.Empty,
            Type = _availableTypes.FirstOrDefault(t => t.Id == "list") ?? _availableTypes.First()
        };

        StateHasChanged();
    }
    
    private async Task ConfirmDelete(GraphNodeWithCount node)
    {
        var parameters = new DialogParameters { ["Node"] = node };
        var options = new DialogOptions { CloseButton = true };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete item?", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            await DeleteNode(node);
        }
    }

    private async Task DeleteNode(GraphNodeWithCount node)
    {
        if (node.Type is { AllowDeletion: false })
        {
            Console.WriteLine($"Skipping deletion: {node.Title} is a protected type.");
            return;
        }

        var edge = await Db.Edges.FirstOrDefaultAsync(e => e.FromId == CurrentNode.Id && e.ToId == node.Id);
        var toNode = await Db.Nodes.FirstOrDefaultAsync(n => n.Id == node.Id);

        if (edge != null) Db.Edges.Remove(edge);
        if (toNode != null) Db.Nodes.Remove(toNode);

        await Db.SaveChangesAsync();

        _children?.RemoveAll(c => c.Id == node.Id);
        StateHasChanged();
    }

    private async Task HandleSwipeEnd(SwipeEventArgs e, GraphNodeWithCount node)
    {
        if (App.IsMobile && e.SwipeDirection == SwipeDirection.RightToLeft && node.Type.AllowDeletion)
        {
            await ConfirmDelete(node);
        }
    }
    private async Task HandleFilterChanged(string text)
    {
        _newNode.Title = text;
        await InvokeAsync(StateHasChanged);
    }

    private bool _isHidingFooter = false;

    private async Task Reset()
    {
        _isHidingFooter = true;
        await Task.Delay(250); // Match animation duration
        await OnResetFooter.InvokeAsync();
        _isHidingFooter = false;
        _newNode = new GraphNode
        {
            Title = string.Empty,
            Type = _availableTypes.FirstOrDefault(t => t.Id == "list") ?? _availableTypes.First()
        };
    }

    private class GraphNodeWithCount : GraphNode
    {
        public int ChildCount { get; set; }
    }



}
