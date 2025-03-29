namespace LowlandTech.Vylyr.Layout;

public partial class NewNode : ComponentBase
{
    [Parameter] public List<GraphNodeType> NodeTypes { get; set; } = [];
    [Inject] private IJSRuntime Js { get; set; } = default!;
    [Parameter] public EventCallback<GraphNode> OnCreate { get; set; }
    [Parameter] public GraphNode? Node { get; set; }
    [CascadingParameter] private IMudDialogInstance? MudDialog { get; set; }

    private MudTextField<string>? _inputRef;

    private async Task OnInputFocus(FocusEventArgs args)
    {
        await Js.InvokeVoidAsync("scrollInputIntoView", _inputRef!.InputReference!.ElementReference);
    }

    private async Task HandleEnter(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
            await CreateNode();
    }

    private async Task CreateNode()
    {
        if (!string.IsNullOrWhiteSpace(Node?.Title))
        {
            await OnCreate.InvokeAsync(Node);
            MudDialog!.Close();
        }
    }

    protected override void OnInitialized()
    {
        Node!.Type = NodeTypes.FirstOrDefault(t => t.Id == "list") ?? NodeTypes.First();
    }
}
