namespace LowlandTech.Vylyr.Layout;

public partial class NavMenu
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public AppVm App { get; set; } = default!;
    [Inject] public NavMenuVm NavVm { get; set; } = default!;
    [Inject] public GraphContext Db { get; set; } = default!;

    private FooterMode _footerMode = FooterMode.None;
    private List<GraphNodeType> _availableTypes = [];
    private List<GraphNodeWithCount>? _children;
    private GraphNode _newNode = new();

    private async Task OpenNewNodeDialog()
    {
        var parameters = new DialogParameters
        {
            { "NodeTypes", _availableTypes },
            { "Node", _newNode },
            { "OnCreate", EventCallback.Factory.Create<GraphNode>(this, HandleCreateNode) }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        await DialogService.ShowAsync<NewNode>("New item", parameters, options);
    }

    private async Task HandleCreateNode()
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
        Db.Edges.Add(new GraphEdge { FromId = App.CurrentNode!.Id, ToId = newNode.Id });

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

    private void SetFooterMode(FooterMode mode)
    {
        _footerMode = mode;
    }

    private void ResetFooterMode()
    {
        _footerMode = FooterMode.None;
    }

    protected override async Task OnInitializedAsync()
    {
        _availableTypes = await Db.NodeTypes.OrderBy(t => t.Label).ToListAsync();
        var defaultType = _availableTypes.FirstOrDefault(t => t.Id == "list") ?? _availableTypes.First();

        App.OnChange += () => InvokeAsync(StateHasChanged);
        App.OnMenuChange += () => InvokeAsync(StateHasChanged);
        await App.InitMenuAsync(Db);
    }

    public void Dispose()
    {
        App.OnChange -= () => InvokeAsync(StateHasChanged);
        App.OnMenuChange -= () => InvokeAsync(StateHasChanged);
    }
}
