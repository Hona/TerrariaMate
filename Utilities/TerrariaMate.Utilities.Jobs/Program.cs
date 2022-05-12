using TerrariaMate.Utilities.Jobs;

Log.Title();

var jobHost = new JobHost();

var cancellationToken = ConsoleHelper.GetExitCancellationToken();

try
{
    await jobHost.RunAsync(cancellationToken);
}
catch (TaskCanceledException e)
{
    Console.WriteLine();
    Log.Warning("Cancellation requested, quitting...");
}
catch (Exception e)
{
    Console.WriteLine();
    Log.Exception("Unhandled exception, quitting...", e);
}
