using Ardalis.GuardClauses;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using TerrariaMate.CDN;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CDNConfig>(builder.Configuration.GetSection(nameof(CDNConfig)));

var app = builder.Build();

var config = app.Services.GetRequiredService<IOptions<CDNConfig>>().Value;

Guard.Against.NullOrWhiteSpace(config.LatestTerrariaVersion, nameof(config.LatestTerrariaVersion));
Guard.Against.NullOrWhiteSpace(config.StaticFilesRoot, nameof(config.StaticFilesRoot));

app.UseHttpsRedirection();

// Add Latest files
var latestFileProvider = new PhysicalFileProvider(Path.Join(config.StaticFilesRoot, "v" + config.LatestTerrariaVersion));
app.UseFileServer(new FileServerOptions
{
    FileProvider = latestFileProvider,
    RedirectToAppendTrailingSlash = true,
    EnableDirectoryBrowsing = true,
    RequestPath = "/latest"
});

// Add all version files (including latest with the raw version)
var rootFileProvider = new PhysicalFileProvider(config.StaticFilesRoot);
app.UseFileServer(new FileServerOptions
{
    FileProvider = rootFileProvider,
    RedirectToAppendTrailingSlash = true,
    EnableDirectoryBrowsing = true,
});

app.Run();