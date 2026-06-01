using LateRi.BusBookingSystem.ConsoleUI.Enums;

namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Bus : BaseEntity
{
    public string BusId => Id;
    public string CoachName { get; }
    public BusClassification Classification { get; }
    public int TotalSeats { get; }

    public int SeatsPerRow => Classification == BusClassification.Economy ? 4 : 3;
    private char[] AllColumns => Classification == BusClassification.Economy ? ['A', 'B', 'C', 'D'] : ['A', 'B', 'C'];

    private int TotalRows => (int)Math.Ceiling((double)TotalSeats / SeatsPerRow);

    public IReadOnlyList<string> AllSeats => _allSeats;

    private readonly HashSet<string> _reservedSeats = [];
    private readonly List<string> _allSeats;

    public Bus(string coachNumber, BusClassification classification, int totalSeats = 0)
    {
        CoachName = coachNumber;
        Classification = classification;
        TotalSeats = totalSeats > 0 ? totalSeats : (int)classification;
        _allSeats = GenerateAllSeats();
    }

    private List<string> GenerateAllSeats()
    {
        var seats = new List<string>();
        var count = 0;
        for (var row = 1; row <= TotalRows && count < TotalSeats; row++)
        {
            foreach (var col in AllColumns)
            {
                if (count >= TotalSeats) break;
                seats.Add($"{col}{row}");
                count++;
            }
        }
        return seats;
    }

    public bool IsValidSeat(string seatCode) => _allSeats.Contains(seatCode);

    public bool IsSeatAvailable(string seatCode) =>
        !_reservedSeats.Contains(seatCode);

    public bool ReserveSeat(string seatCode)
    {
        if (!IsSeatAvailable(seatCode)) return false;
        _reservedSeats.Add(seatCode);
        return true;
    }

    public List<string> GetAvailableSeats() =>
        _allSeats.Where(s => !_reservedSeats.Contains(s)).ToList();

    public override string GetSummary() =>
        $"[{BusId}] Coach: {CoachName} | Class: {Classification} | " +
        $"Seats: {TotalSeats - _reservedSeats.Count}/{TotalSeats} available";

    public override string ToString() => GetSummary();

    public bool ClearReservedSeat(string seatCode)
    {
        if (!_reservedSeats.Contains(seatCode)) return false;
        _reservedSeats.Remove(seatCode);
        return true;
    }
}
