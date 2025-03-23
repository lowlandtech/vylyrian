namespace LowlandTech.Vylyr.Core.Infrastructure;

public class GraphContext(DbContextOptions<GraphContext> options) : DbContext(options)
{
    public DbSet<GraphNode> Nodes => Set<GraphNode>();
    public DbSet<GraphEdge> Edges => Set<GraphEdge>();
    public DbSet<NodeType> NodeTypes => Set<NodeType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GraphEdge>()
            .HasOne(e => e.From)
            .WithMany(n => n.OutgoingEdges)
            .HasForeignKey(e => e.FromId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GraphEdge>()
            .HasOne(e => e.To)
            .WithMany()
            .HasForeignKey(e => e.ToId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GraphNode>()
            .HasOne(n => n.Type)
            .WithMany()
            .HasForeignKey(n => n.TypeId);
    }
}
