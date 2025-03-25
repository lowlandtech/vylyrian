namespace LowlandTech.Vylyr.Models;

public class GraphNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string TypeId { get; set; } = default!;
    public GraphNodeType Type { get; set; } = default!;

    public string? Route { get; set; }

    public ICollection<GraphEdge> OutgoingEdges { get; set; } = new List<GraphEdge>();
}