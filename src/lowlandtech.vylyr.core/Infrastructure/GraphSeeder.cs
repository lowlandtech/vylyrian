namespace LowlandTech.Vylyr.Core.Infrastructure;

public static class GraphSeeder
{
    public static async Task UseCaseData(this GraphContext db)
    {
        if (await db.Nodes.AnyAsync()) return;

        var user = new GraphNode { Id = "user", Title = "User", Type = "user" };
        var list1 = new GraphNode { Id = "l1", Title = "Projects", Type = "list" };
        var list2 = new GraphNode { Id = "l2", Title = "Tasks", Type = "list" };
        var list3 = new GraphNode { Id = "l3", Title = "Completed", Type = "list" };

        db.Nodes.AddRange(user, list1, list2, list3);
        db.Edges.AddRange(
            new GraphEdge { From = user, To = list1 },
            new GraphEdge { From = list1, To = list2 },
            new GraphEdge { From = list2, To = list3 }
        );

        await db.SaveChangesAsync();
    }
}