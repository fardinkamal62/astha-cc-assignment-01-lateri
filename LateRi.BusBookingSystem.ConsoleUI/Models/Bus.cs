namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public enum BusClassification
{
    Business,
    Economy
}

public class Bus
{
    private static int _nextId = 1;

    private static readonly Dictionary<BusClassification, int> CapacityMap = new()
    {
        { BusClassification.Business, 20 },
        { BusClassification.Economy, 40 }
    };

    public int Id { get; }
    public string CoachNumber { get; }
    public BusClassification Classification { get; }
    public int TotalSeats { get; }

    public Bus(string coachNumber, BusClassification classification)
    {
        Id = _nextId++;
        CoachNumber = coachNumber;
        Classification = classification;
        TotalSeats = CapacityMap[classification];
    }

    public override string ToString() =>
        $"[{Id}] Coach {CoachNumber} | {Classification} ({TotalSeats} seats)";
}
