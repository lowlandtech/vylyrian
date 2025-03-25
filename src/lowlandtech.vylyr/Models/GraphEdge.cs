namespace LowlandTech.Vylyr.Models;

public class GraphEdge
{
    public int Id { get; set; }
    public string FromId { get; set; } = default!;
    public GraphNode From { get; set; } = default!;

    public string ToId { get; set; } = default!;
    public GraphNode To { get; set; } = default!;
}
