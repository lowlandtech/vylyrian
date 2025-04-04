namespace LowlandTech.Vylyr.Layout;

public partial class NavMenu
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public AppVm App { get; set; } = default!;
    [Inject] public NavMenuVm NavVm { get; set; } = default!;
    [Inject] public GraphContext Db { get; set; } = default!;

    private List<GraphNodeType> _availableTypes = [];
    private List<GraphNodeWithCount>? _children;

    private async Task OpenNewNodeDialog()
    {
        var parameters = new DialogParameters
        {
            { "NodeTypes", _availableTypes },
            { "Node", NavVm.NewNode },
            { "OnCreate", EventCallback.Factory.Create<GraphNode>(this, HandleCreateNode) }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        await DialogService.ShowAsync<NewNode>("New item", parameters, options);
    }

    private async Task HandleCreateNode()
    {
        if (string.IsNullOrWhiteSpace(NavVm.NewNode.Title))
            return;

        var newNode = new GraphNode
        {
            Id = Guid.NewGuid().ToString(),
            Title = NavVm.NewNode.Title,
            Type = NavVm.NewNode.Type
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
        NavVm.ResetNewNode(_availableTypes);

        StateHasChanged();
    }

    private void SetFooterMode(FooterMode mode)
    {
        NavVm.SetFooterMode(mode);
    }

    private void ResetFooterMode()
    {
        NavVm.ResetFooter();
    }

    protected override async Task OnInitializedAsync()
    {
        _availableTypes = await Db.NodeTypes.OrderBy(t => t.Label).ToListAsync();
        var defaultType = _availableTypes.FirstOrDefault(t => t.Id == "list") ?? _availableTypes.First();

        App.OnChange += () => InvokeAsync(StateHasChanged);
        App.OnMenuChange += () => InvokeAsync(StateHasChanged);
        NavVm.OnChange += () => InvokeAsync(StateHasChanged);
        await App.InitMenuAsync(Db);
    }

    public void Dispose()
    {
        App.OnChange -= () => InvokeAsync(StateHasChanged);
        App.OnMenuChange -= () => InvokeAsync(StateHasChanged);
        NavVm.OnChange -= () => InvokeAsync(StateHasChanged);
    }
}
