namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Schedule
{
    private static int _nextId = 1;

    public int Id { get; }
    public int BusId { get; }
    public string From { get; }
    public string To { get; }
    public DateTime Departure { get; }
    public decimal Price { get; }

    public Schedule(int busId, string from, string to, DateTime departure, decimal price)
    {
        Id = _nextId++;
        BusId = busId;
        From = from;
        To = to;
        Departure = departure;
        Price = price;
    }

    public override string ToString() =>
        $"[{Id}] {From} → {To} | {Departure:g} | ${Price:F2} (Bus #{BusId})";
}
