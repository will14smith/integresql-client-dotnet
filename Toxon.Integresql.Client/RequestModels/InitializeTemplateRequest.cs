using System.Text.Json.Serialization;

namespace Toxon.Integresql.Client.RequestModels;

internal class InitializeTemplateRequest
{
    [JsonPropertyName("hash")] public string? Hash { get; set; }
}