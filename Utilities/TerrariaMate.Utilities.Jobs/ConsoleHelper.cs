namespace TerrariaMate.Utilities.Jobs;

public static class ConsoleHelper
{
    public static CancellationToken GetExitCancellationToken()
    {
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            cts.Cancel();
            e.Cancel = true;
        };

        return cts.Token;
    }
}