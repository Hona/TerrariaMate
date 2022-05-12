using System.Collections.Immutable;
using System.Diagnostics;
using Spectre.Console;

namespace TerrariaMate.Utilities.Jobs;

public class JobHost
{
    private ImmutableArray<Type> _availableJobs;

    public JobHost()
    {
        Log.Info("Initializing Job Host...");

        var type = typeof(IJob);

        _availableJobs = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsClass && type.IsAssignableFrom(p))
            .ToImmutableArray();

        Log.Info($"Done - Loaded {_availableJobs.Length} jobs.");
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Log.Info("Please select a job to run.");

        var selectedJob = await new SelectionPrompt<Type>()
            .Title("Available Jobs")
            .PageSize(10)
            .MoreChoicesText("[grey](Up & Down for more jobs)[/]")
            .AddChoices(_availableJobs)
            .UseConverter(x => x.Name)
            .ShowAsync(AnsiConsole.Console, cancellationToken);

        Log.Info($"Running {selectedJob.Name}...");

        // Asynchronous
        var jobTime = Stopwatch.StartNew();

        await AnsiConsole.Status()
            .StartAsync($"Running {selectedJob.Name}...", async ctx =>
            {
                var selectedJobInstance = (IJob)Activator.CreateInstance(selectedJob)!;
                await selectedJobInstance.RunAsync(cancellationToken);
            });

        jobTime.Stop();

        Log.Info($"Done - Job complete in {jobTime.ElapsedMilliseconds}ms.");

    }
}