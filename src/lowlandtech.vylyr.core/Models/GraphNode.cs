namespace LowlandTech.Vylyr.Core.Models;

public class GraphNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = "list"; // or "user", "action", etc.
    public string? Route { get; set; }

    public ICollection<GraphEdge> OutgoingEdges { get; set; } = new List<GraphEdge>();
}