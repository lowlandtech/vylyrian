namespace LowlandTech.Vylyr.Infrastructure;

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
                        Id = "users", 
                        Label = "Users", 
                        Icon = Icons.Material.Filled.List, 
                        ComponentName = "UserView"
                    },
                    new GraphNodeType
                    {
                        Id = "list", 
                        Label = "List", 
                        Icon = Icons.Material.Filled.List, 
                        ComponentName = "ListView"
                    },
                    new GraphNodeType
                    {
                        Id = "action", 
                        Label = "Action", 
                        Icon = Icons.Material.Filled.FlashOn,
                        ComponentName = "ActionView"
                    },
                    new GraphNodeType
                    {
                        Id = "report", 
                        Label = "Report", 
                        Icon = Icons.Material.Filled.Assessment,
                        ComponentName = "ReportView"
                    },
                    new GraphNodeType
                    {
                        Id = "task",
                        Label = "Task",
                        Icon = Icons.Material.Filled.Task,
                        ComponentName = "TaskView"
                    },
                    new GraphNodeType
                    {
                        Id = "page",
                        Label = "Page",
                        Icon = Icons.Material.Filled.Pages,
                        ComponentName = "HomeView"
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
                var home = new GraphNode { Id = "/", Title = "Home", TypeId = "page" };
                var counter = new GraphNode { Id = "counter", Title = "Counter", TypeId = "page" };
                var weather = new GraphNode { Id = "weather", Title = "Weather", TypeId = "page" };
                var list1 = new GraphNode { Id = "l1", Title = "Projects", TypeId = "list" };
                var list2 = new GraphNode { Id = "l2", Title = "Tasks", TypeId = "list" };
                var list3 = new GraphNode { Id = "l3", Title = "Completed", TypeId = "list" };

                var task1 = new GraphNode { Id = "t1", Title = "Task 1", TypeId = "task" };
                var task2 = new GraphNode { Id = "t2", Title = "Task 2", TypeId = "task" };
                var task3 = new GraphNode { Id = "t3", Title = "Task 3", TypeId = "task" };

                db.Nodes.AddRange(user, list1, action, list2, list3, task1, task2, task3, home, counter, weather);
                await db.SaveChangesAsync();

                if (!await db.Edges.AnyAsync())
                {
                    db.Edges.Add(new GraphEdge { From = user, To = list1 });
                    db.Edges.Add(new GraphEdge { From = user, To = home });
                    db.Edges.Add(new GraphEdge { From = user, To = counter });
                    db.Edges.Add(new GraphEdge { From = user, To = weather });
                    db.Edges.Add(new GraphEdge { From = list1, To = action });
                    db.Edges.Add(new GraphEdge { From = list1, To = list2 });
                    db.Edges.Add(new GraphEdge { From = list2, To = list3 });

                    db.Edges.Add(new GraphEdge { From = list2, To = task1 });
                    db.Edges.Add(new GraphEdge { From = list2, To = task2 });
                    db.Edges.Add(new GraphEdge { From = list2, To = task3 });

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