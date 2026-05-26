namespace LateRi.BusBookingSystem.ConsoleUI.Models;

using Enums;

public class Bus : BaseEntity
{
    public string BusId => Id;
    public string CoachNumber { get; private set; }
    public BusClassification Classification { get; private set; }
    public int TotalSeats => (int)Classification;

    private readonly HashSet<string> _reservedSeats = new();

    public IReadOnlyCollection<string> ReservedSeats => _reservedSeats;

    public Bus(string coachNumber, BusClassification classification)
    {
        CoachNumber = coachNumber;
        Classification = classification;
    }

    public bool IsSeatAvailable(string seatNumber) =>
        !_reservedSeats.Contains(seatNumber);

    public bool ReserveSeat(string seatNumber)
    {
        if (!IsSeatAvailable(seatNumber)) return false;
        _reservedSeats.Add(seatNumber);
        return true;
    }

    public List<string> GetAvailableSeats()
    {
        var all = Enumerable.Range(1, TotalSeats)
            .Select(i => $"S{i:D2}")
            .ToList();
        return all.Where(s => !_reservedSeats.Contains(s)).ToList();
    }

    public override string GetSummary() =>
        $"[{BusId}] Coach: {CoachNumber} | Class: {Classification} | " +
        $"Seats: {TotalSeats - _reservedSeats.Count}/{TotalSeats} available";

    public override string ToString() => GetSummary();
}
