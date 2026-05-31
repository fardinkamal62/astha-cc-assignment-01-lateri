using LateRi.BusBookingSystem.ConsoleUI.Enums;

namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Bus : BaseEntity
{
    public string BusId => Id;
    public string CoachNumber { get; }
    public BusClassification Classification { get; }
    public int TotalSeats => (int)Classification;

    public int SeatsPerRow => Classification == BusClassification.Economy ? 4 : 3;
    private char[] LeftColumns => Classification == BusClassification.Economy ? ['A', 'B'] : ['A'];
    private char[] RightColumns => Classification == BusClassification.Economy ? ['C', 'D'] : ['B', 'C'];
    private char[] AllColumns => Classification == BusClassification.Economy ? ['A', 'B', 'C', 'D'] : ['A', 'B', 'C'];

    private int TotalRows => (int)Math.Ceiling((double)TotalSeats / SeatsPerRow);

    private readonly HashSet<string> _reservedSeats = [];
    private readonly List<string> _allSeats;

    public Bus(string coachNumber, BusClassification classification)
    {
        CoachNumber = coachNumber;
        Classification = classification;
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

    public string GetSeatLayout()
    {
        var maxWidth = _allSeats.Max(s => s.Length);
        var rows = new List<string>();
        var count = 0;

        rows.Add($"{"Left".PadRight(maxWidth * 2 + 1)} | {"Right".PadRight(maxWidth * 2 - 1)}");
        rows.Add(new string('-', maxWidth * 4 + 3));

        for (var row = 1; row <= TotalRows && count < TotalSeats; row++)
        {
            var left = new List<string>();
            var right = new List<string>();

            foreach (var col in AllColumns)
            {
                if (count >= TotalSeats) break;
                var seat = $"{col}{row}";
                var formatted = _reservedSeats.Contains(seat)
                    ? new string('X', seat.Length).PadLeft(maxWidth)
                    : seat.PadLeft(maxWidth);
                if (LeftColumns.Contains(col))
                    left.Add(formatted);
                else
                    right.Add(formatted);
                count++;
            }

            rows.Add($"{string.Join(" ", left)} | {string.Join(" ", right)}");
        }

        return string.Join(Environment.NewLine, rows);
    }

    public override string GetSummary() =>
        $"[{BusId}] Coach: {CoachNumber} | Class: {Classification} | " +
        $"Seats: {TotalSeats - _reservedSeats.Count}/{TotalSeats} available";

    public override string ToString() => GetSummary();

    public bool ClearReservedSeat(string seatCode)
    {
        if (!_reservedSeats.Contains(seatCode)) return false;
        _reservedSeats.Remove(seatCode);
        return true;
    }
}
