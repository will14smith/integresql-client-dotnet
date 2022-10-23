using System.Text.Json.Serialization;

namespace Toxon.Integresql.Client.RequestModels;

internal class DatabaseResponse
{
    [JsonPropertyName("templateHash")] public string? TemplateHash { get; set; }
    [JsonPropertyName("config")] public DatabaseConfigResponse? Config { get; set; }
}