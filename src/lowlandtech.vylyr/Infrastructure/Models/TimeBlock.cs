namespace LowlandTech.Vylyr.Infrastructure.Models;

public class TimeBlock
{
    public DateTime Date { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<TimeSlot> Slots { get; set; } = new();

    public string Group => Date.ToString("MMMM yyyy");         // e.g. "March 2025"
    public string DayOfWeek => Date.ToString("ddd");           // e.g. "Mon"
    public string DayOfMonth => Date.Day.ToString();           // e.g. "25"

    public TimeBlock(DateTime date)
    {
        Date = date;
        for (int i = 0; i < 24; i++)
        {
            Slots.Add(new TimeSlot { Hour = i });
        }
    }
}