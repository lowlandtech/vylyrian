﻿namespace LowlandTech.Vylyr.Models;

public class GraphNodeType
{
    public string Id { get; set; } = default!; // e.g. "list", "action", "report"
    public string Label { get; set; } = default!;
    public string Icon { get; set; } = "Icons.Material.Filled.HelpOutline"; // MudBlazor icon reference
    public string? ComponentName { get; set; } // Fully qualified or key for dynamic component loading
    public bool AllowDeletion { get; set; } = true; // Default true for backwards compatibility
}
