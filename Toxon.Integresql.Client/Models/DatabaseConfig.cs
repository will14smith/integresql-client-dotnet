using System.Collections.Generic;

namespace Toxon.Integresql.Client.Models;

public class DatabaseConfig
{
    public DatabaseConfig(string host, int port, string? username, string? password, string database, IReadOnlyDictionary<string, string> additionalParams)
    {
        Host = host;
        Port = port;
        Username = username;
        Password = password;
        Database = database;
        AdditionalParams = additionalParams;
    }

    public string Host { get; }
    public int Port { get; }
    public string? Username { get; }
    public string? Password { get; }
    public string Database { get; }
    public IReadOnlyDictionary<string, string> AdditionalParams { get; }
}