using System.Diagnostics;

namespace TerrariaMate.Wiki.Core.Models;

[DebuggerDisplay("{Id}", Name = "{DisplayName} ({InternalName})")]
public record ItemMetadata(int Id, string DisplayName, string InternalName);