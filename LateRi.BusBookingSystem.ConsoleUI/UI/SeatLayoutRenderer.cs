using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.UI;

public static class SeatLayoutRenderer
{
    public static string Render(Bus bus)
    {
        var seats = bus.AllSeats;
        var maxWidth = seats.Max(s => s.Length);
        var seatsPerRow = bus.SeatsPerRow;
        var leftCount = seatsPerRow / 2;
        var output = new List<string>();

        var leftWidth = leftCount * maxWidth + Math.Max(0, leftCount - 1);
        var rightWidth = (seatsPerRow - leftCount) * maxWidth + Math.Max(0, seatsPerRow - leftCount - 1);
        output.Add($"{"Left".PadRight(Math.Max(leftWidth, "Left".Length))} | {"Right".PadRight(Math.Max(rightWidth, "Right".Length))}");
        output.Add(new string('-', leftWidth + 3 + rightWidth));

        var idx = 0;
        while (idx < seats.Count)
        {
            var rowSeats = seats.Skip(idx).Take(seatsPerRow).ToList();
            var left = rowSeats.Take(leftCount)
                .Select(s => (bus.IsSeatAvailable(s) ? s : new string('X', s.Length)).PadLeft(maxWidth))
                .ToList();
            var right = rowSeats.Skip(leftCount)
                .Select(s => (bus.IsSeatAvailable(s) ? s : new string('X', s.Length)).PadLeft(maxWidth))
                .ToList();

            output.Add($"{string.Join(" ", left)} | {string.Join(" ", right)}");
            idx += rowSeats.Count;
        }

        return string.Join(Environment.NewLine, output);
    }
}
