namespace LowlandTech.Vylyr.Core.Infrastructure.Models;

public class TimeSlot
{
    public int Hour { get; set; }
    public string Display => $"{Hour:D2}:00";
    public bool IsSelected { get; set; } = false;
}