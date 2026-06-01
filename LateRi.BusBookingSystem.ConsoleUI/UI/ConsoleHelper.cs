namespace LateRi.BusBookingSystem.ConsoleUI.UI;

public static class ConsoleHelper
{
    public static void Write(string text)
    {
        Console.Write(text);
    }

    public static void WriteColor(string text, ConsoleColor color)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = prev;
    }

    public static void WriteLineColor(string text, ConsoleColor color)
    {
        WriteColor(text + Environment.NewLine, color);
    }

    public static void WriteBanner(string text)
    {
        const string corner = "╔═══════════════════════════════════════╗";
        const string bottom = "╚═══════════════════════════════════════╝";
        var padding = (corner.Length - 2 - text.Length) / 2;
        var line = "║" + new string(' ', Math.Max(0, padding)) + text +
                   new string(' ', Math.Max(0, corner.Length - 2 - padding - text.Length)) + "║";
        Console.WriteLine(corner);
        WriteColor(line, ConsoleColor.Cyan);
        Console.WriteLine();
        Console.WriteLine(bottom);
    }

    public static void WriteHeader(string text)
    {
        Console.WriteLine();
        WriteColor($"  {text}", ConsoleColor.Yellow);
        Console.WriteLine();
        WriteColor($"  {new string('─', text.Length)}", ConsoleColor.DarkYellow);
        Console.WriteLine();
    }

    public static void WriteSuccess(string text)
    {
        WriteLineColor($"  \u2714 {text}", ConsoleColor.Green);
    }

    public static void WriteWarning(string text)
    {
        WriteLineColor($"  \u26a0 {text}", ConsoleColor.Yellow);
    }

    public static void WriteError(string text)
    {
        WriteLineColor($"  \u2718 {text}", ConsoleColor.Red);
    }

    public static void WriteInfo(string text)
    {
        WriteLineColor($"  {text}", ConsoleColor.Gray);
    }

    public static void WriteSeparator()
    {
        Console.WriteLine($"  {new string('\u2500', 50)}");
    }

    public static void WritePrompt(string text)
    {
        WriteColor($"  {text}", ConsoleColor.White);
    }

    public static void Pause()
    {
        WriteInfo("\nPress any key to continue...");
        Console.ReadKey();
    }

    public static void PrintList<T>(IReadOnlyList<T> items, Func<T, string> label)
    {
        for (var i = 0; i < items.Count; i++)
            WriteLineColor($"{i + 1}. {label(items[i])}", ConsoleColor.Gray);
    }

    public static T? SelectFromList<T>(IReadOnlyList<T> items, Func<T, string> label, string prompt)
        where T : class
    {
        if (items.Count == 0) return null;
        PrintList(items, label);
        WritePrompt(prompt);
        if (!int.TryParse(Console.ReadLine(), out var choice)) return null;
        if (choice < 1 || choice > items.Count) return null;
        return items[choice - 1];
    }
}
