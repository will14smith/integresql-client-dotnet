using System.Text.Json.Serialization;

namespace Toxon.Integresql.Client.RequestModels;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(InitializeTemplateRequest))]
[JsonSerializable(typeof(InitializeTemplateResponse))]
[JsonSerializable(typeof(GetDatabaseResponse))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}