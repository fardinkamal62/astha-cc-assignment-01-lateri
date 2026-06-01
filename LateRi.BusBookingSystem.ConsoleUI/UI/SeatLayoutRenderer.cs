using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.UI;

public static class SeatLayoutRenderer
{
    public static void Render(Bus bus)
    {
        var seats = bus.AllSeats;
        var seatsPerRow = bus.SeatsPerRow;
        var leftCount = seatsPerRow / 2;
        var maxWidth = seats.Max(s => s.Length);

        var leftWidth = leftCount * (maxWidth + 1) - 1;
        var rightWidth = (seatsPerRow - leftCount) * (maxWidth + 1) - 1;
        var totalWidth = leftWidth + 3 + rightWidth;

        // top border
        ConsoleHelper.WriteColor($"  \u2554{new string('\u2550', totalWidth)}\u2557\n", ConsoleColor.DarkGray);

        // header row
        ConsoleHelper.WriteColor("  \u2551 ", ConsoleColor.DarkGray);
        ConsoleHelper.WriteColor("Left".PadRight(leftWidth), ConsoleColor.Yellow);
        ConsoleHelper.WriteColor(" \u2502 ", ConsoleColor.DarkGray);
        ConsoleHelper.WriteColor("Right".PadRight(rightWidth), ConsoleColor.Yellow);
        ConsoleHelper.WriteColor(" \u2551\n", ConsoleColor.DarkGray);

        // separator
        ConsoleHelper.WriteColor($"  \u2560{new string('\u2550', leftWidth)}\u256c{new string('\u2550', rightWidth)}\u2563\n", ConsoleColor.DarkGray);

        var idx = 0;
        while (idx < seats.Count)
        {
            var rowSeats = seats.Skip(idx).Take(seatsPerRow).ToList();
            var left = rowSeats.Take(leftCount).ToList();
            var right = rowSeats.Skip(leftCount).ToList();

            ConsoleHelper.WriteColor("  \u2551 ", ConsoleColor.DarkGray);
            WriteSeats(left, bus, maxWidth);
            ConsoleHelper.WriteColor(" \u2502 ", ConsoleColor.DarkGray);
            WriteSeats(right, bus, maxWidth);
            ConsoleHelper.WriteColor(" \u2551\n", ConsoleColor.DarkGray);

            idx += rowSeats.Count;
        }

        // bottom border
        ConsoleHelper.WriteColor($"  \u255a{new string('\u2550', totalWidth)}\u255d\n", ConsoleColor.DarkGray);

        // legend
        ConsoleHelper.WriteColor("  \u2588 ", ConsoleColor.Green);
        Console.Write("Available  ");
        ConsoleHelper.WriteColor("\u2588 ", ConsoleColor.Red);
        Console.Write("Taken");
        Console.WriteLine();
    }

    private static void WriteSeats(List<string> seats, Bus bus, int maxWidth)
    {
        for (var i = 0; i < seats.Count; i++)
        {
            if (i > 0) Console.Write(" ");
            var seat = seats[i];
            var available = bus.IsSeatAvailable(seat);
            var color = available ? ConsoleColor.Green : ConsoleColor.Red;
            var label = available ? seat : new string('X', seat.Length);
            ConsoleHelper.WriteColor(label.PadLeft(maxWidth), color);
        }
    }
}
