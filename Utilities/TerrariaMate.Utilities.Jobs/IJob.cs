namespace TerrariaMate.Utilities.Jobs;

public interface IJob
{
    Task RunAsync(CancellationToken cancellationToken = default);
}