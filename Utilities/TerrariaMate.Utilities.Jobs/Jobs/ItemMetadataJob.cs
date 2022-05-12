using System.Text.Json;
using Humanizer;
using TerrariaMate.Wiki.Core.Models;

namespace TerrariaMate.Utilities.Jobs.Jobs;

public class ItemMetadataJob : IJob
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Log.Info("Loading raw item metadata...");
        var rawText = await File.ReadAllTextAsync(Constants.InputFolderPath + "ItemIds.txt", cancellationToken);

        var lines = rawText.Split('\n');

        var itemMetadatas = new List<ItemMetadata>();

        foreach (var line in lines.Skip(1))
        {
            try
            {
                var parts = line.Split('\t');

                var itemMetadata = new ItemMetadata(int.Parse(parts[0]), parts[1].Trim(), parts[2].Trim());
                itemMetadatas.Add(itemMetadata);
            }
            catch (Exception)
            {
                Log.Error("Error occurred while parsing line: " + line);
                throw;
            }
        }

        Log.Info("Saving item metadata...");
        await using var fileStream = File.Create(Constants.OutputFolderPath + "ItemMetadata.json");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        await JsonSerializer.SerializeAsync(fileStream, itemMetadatas, options, cancellationToken);

        Log.Info($"{fileStream.Position.Bytes()} written.");
    }
}