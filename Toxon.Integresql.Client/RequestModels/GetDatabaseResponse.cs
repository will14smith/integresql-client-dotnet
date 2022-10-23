using System.Text.Json.Serialization;
using Toxon.Integresql.Client.Exceptions;
using Toxon.Integresql.Client.Models;

namespace Toxon.Integresql.Client.RequestModels;

internal class GetDatabaseResponse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("database")] public DatabaseResponse? Database { get; set; }

    public TestDatabase ToModel() => new(
        Id, 
        Database?.TemplateHash ?? throw new InternalErrorException("database hash in response was null"), 
        Database?.Config?.ToModel() ?? throw new InternalErrorException("database config in response was null")
    );
}