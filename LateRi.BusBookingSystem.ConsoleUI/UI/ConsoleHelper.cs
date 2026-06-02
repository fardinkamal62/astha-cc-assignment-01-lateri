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

    public static void WriteLine() => Console.WriteLine();

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

    public static T? SelectFromTable<T>(IReadOnlyList<T> items, string[] headers,
        Func<T, int, string?[]> rowRenderer, string prompt) where T : class
    {
        if (items.Count == 0) return null;
        PrintTable(items, headers, rowRenderer);
        WritePrompt(prompt);
        if (!int.TryParse(Console.ReadLine(), out var choice)) return null;
        if (choice < 1 || choice > items.Count) return null;
        return items[choice - 1];
    }

    public static void PrintCard(string title, params (string label, string value)[] fields)
    {
        var labelPad = fields.Max(f => f.label.Length);
        var valPad = fields.Max(f => f.value.Length);
        var fieldLen = labelPad + 3 + valPad;
        var inner = Math.Max(title.Length + 2, fieldLen) + 2;

        WriteColor($"  ╔{new string('═', inner)}╗\n", ConsoleColor.DarkGray);

        var titleInner = inner - 2;
        var titleLeft = (titleInner - title.Length) / 2;
        WriteColor("  ║ ", ConsoleColor.DarkGray);
        WriteColor(title.PadLeft(titleLeft + title.Length).PadRight(titleInner), ConsoleColor.Cyan);
        WriteColor(" ║\n", ConsoleColor.DarkGray);

        WriteColor($"  ╠{new string('═', inner)}╣\n", ConsoleColor.DarkGray);

        foreach (var (label, value) in fields)
        {
            WriteColor("  ║ ", ConsoleColor.DarkGray);
            WriteColor(label.PadLeft(labelPad), ConsoleColor.Yellow);
            WriteColor(" : ", ConsoleColor.DarkGray);
            Write(value.PadRight(valPad));
            WriteColor(" ║\n", ConsoleColor.DarkGray);
        }

        WriteColor($"  ╚{new string('═', inner)}╝\n", ConsoleColor.DarkGray);
    }

    public static void PrintTable<T>(IReadOnlyList<T> items, string[] headers, Func<T, int, string?[]> rowRenderer)
    {
        if (items.Count == 0) return;

        var colCount = headers.Length;
        var colWidths = headers.Select(h => h.Length).ToArray();
        var rows = new List<string?[]>(items.Count);

        for (var idx = 0; idx < items.Count; idx++)
        {
            var cells = rowRenderer(items[idx], idx + 1);
            rows.Add(cells);
            for (var i = 0; i < Math.Min(cells.Length, colCount); i++)
                if ((cells[i]?.Length ?? 0) > colWidths[i])
                    colWidths[i] = cells[i]!.Length;
        }

        for (var i = 0; i < colCount; i++)
            colWidths[i] += 2;

        Write("  ┌");
        for (var i = 0; i < colCount; i++)
        {
            Write(new string('─', colWidths[i]));
            Write(i < colCount - 1 ? "┬" : "┐");
        }
        WriteLine();

        Write("  │");
        for (var i = 0; i < colCount; i++)
        {
            var w = colWidths[i];
            var text = headers[i];
            var left = (w - text.Length) / 2;
            var right = w - text.Length - left;
            Write(new string(' ', left));
            WriteColor(text, ConsoleColor.Yellow);
            Write(new string(' ', right));
            Write("│");
        }
        WriteLine();

        Write("  ├");
        for (var i = 0; i < colCount; i++)
        {
            Write(new string('─', colWidths[i]));
            Write(i < colCount - 1 ? "┼" : "┤");
        }
        WriteLine();

        foreach (var row in rows)
        {
            Write("  │");
            for (var i = 0; i < colCount; i++)
            {
                var val = i < row.Length ? (row[i] ?? "") : "";
                Write($" {val.PadRight(colWidths[i] - 1)}│");
            }
            WriteLine();
        }

        Write("  └");
        for (var i = 0; i < colCount; i++)
        {
            Write(new string('─', colWidths[i]));
            Write(i < colCount - 1 ? "┴" : "┘");
        }
        WriteLine();
    }
}
