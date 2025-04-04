namespace LowlandTech.Vylyr.ViewModels;

public class NavMenuVm
{
    public FooterMode FooterMode { get; private set; } = FooterMode.None;

    public void SetFooterMode(FooterMode mode)
    {
        FooterMode = mode;
        NotifyChanged();
    }

    public void ResetFooter()
    {
        FooterMode = FooterMode.None;
        NotifyChanged();
    }

    public List<GraphNodeType> AvailableTypes { get; set; } = new();
    public GraphNode NewNode { get; private set; } = new();

    public void ResetNewNode(IEnumerable<GraphNodeType> types)
    {
        AvailableTypes = types.ToList();
        NewNode = new GraphNode
        {
            Title = string.Empty,
            Type = AvailableTypes.FirstOrDefault(t => t.Id == "list") ?? AvailableTypes.FirstOrDefault()
        };
        NotifyChanged();
    }

    public event Action? OnChange;
    private void NotifyChanged() => OnChange?.Invoke();
}
