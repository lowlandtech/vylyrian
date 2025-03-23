namespace LowlandTech.Vylyr.Core.Infrastructure;

public static class GraphSeeder
{
    public static async Task UseCaseData(this GraphContext db)
    {
        try
        {
            if (!await db.NodeTypes.AnyAsync())
            {
                db.NodeTypes.AddRange(
                    new GraphNodeType
                    {
                        Id = "users", Label = "Users", Icon = 
                            Icons.Material.Filled.List, ComponentName = "UserView"
                    },
                    new GraphNodeType
                    {
                        Id = "list", Label = "List", 
                        Icon = Icons.Material.Filled.List, ComponentName = "ListView"
                    },
                    new GraphNodeType
                    {
                        Id = "action", Label = "Action", 
                        Icon = Icons.Material.Filled.FlashOn,
                        ComponentName = "ActionView"
                    },
                    new GraphNodeType
                    {
                        Id = "report", Label = "Report", 
                        Icon = Icons.Material.Filled.Assessment,
                        ComponentName = "ReportView"
                    }
                );

                // Save the types first to establish relationships
                await db.SaveChangesAsync();
            }

            if (!await db.Nodes.AnyAsync())
            {
                // Seed GraphNodes using those type IDs
                var action = new GraphNode { Id = "a1", Title = "Run Sync", TypeId = "action" };
                var user = new GraphNode { Id = "user", Title = "User", TypeId = "users" };
                var list1 = new GraphNode { Id = "l1", Title = "Projects", TypeId = "list" };
                var list2 = new GraphNode { Id = "l2", Title = "Tasks", TypeId = "list" };
                var list3 = new GraphNode { Id = "l3", Title = "Completed", TypeId = "list" };

                db.Nodes.AddRange(user, list1, action, list2, list3);
                await db.SaveChangesAsync();

                if (!await db.Edges.AnyAsync())
                {
                    db.Edges.Add(new GraphEdge { From = user, To = list1 });
                    db.Edges.Add(new GraphEdge { From = list1, To = action });
                    db.Edges.Add(new GraphEdge { From = list1, To = list2 });
                    db.Edges.Add(new GraphEdge { From = list2, To = list3 });

                    await db.SaveChangesAsync();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}