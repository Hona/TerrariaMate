using Spectre.Console;

namespace TerrariaMate.Utilities.Jobs;

public static class Log
{
    public static void Title()
    {
        AnsiConsole.Write(new FigletText("TerrariaMate")
            .LeftAligned());

        AnsiConsole.Write(new FigletText(".Jobs")
            .LeftAligned()
            .Color(Color.Green3_1));

        AnsiConsole.WriteLine();
    }

    public static void Info(string message)
    {
        AnsiConsole.MarkupLine("[bold green]   info:[/] " + message);
    }

    public static void Warning(string message)
    {
        AnsiConsole.MarkupLine("[bold yellow]warning:[/] " + message);
    }

    public static void Error(string message)
    {
        AnsiConsole.MarkupLine("[bold red]  error:[/] " + message);
    }

    public static void Exception(string message, Exception ex)
    {
        AnsiConsole.MarkupLine("[bold red]  fatal:[/] " + message);
        AnsiConsole.WriteException(ex,ExceptionFormats.ShortenEverything);
    }
}