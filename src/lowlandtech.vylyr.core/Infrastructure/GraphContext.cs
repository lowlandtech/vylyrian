namespace LowlandTech.Vylyr.Core.Infrastructure;

public class GraphContext(DbContextOptions<GraphContext> options) : DbContext(options)
{
    public DbSet<GraphNode> Nodes => Set<GraphNode>();
    public DbSet<GraphEdge> Edges => Set<GraphEdge>();

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
    }
}
