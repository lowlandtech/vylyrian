namespace LowlandTech.Vylyr.Layout;

public partial class FilterNode : ComponentBase
{
    [Parameter] public EventCallback<string> OnFilterChanged { get; set; }
    [Inject] private IJSRuntime Js { get; set; } = default!;
    [Parameter] public GraphNode? Node { get; set; }

    private MudTextField<string>? _inputRef;

    private async Task OnInputFocus(FocusEventArgs args)
    {
        await Js.InvokeVoidAsync("scrollInputIntoView", _inputRef!.InputReference!.ElementReference);
    }

    private async Task HandleFilterChanged(KeyboardEventArgs arg)
    {
        await OnFilterChanged.InvokeAsync(Node!.Title);
    }
}
