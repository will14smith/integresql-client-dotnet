using System;
using System.Threading.Tasks;
using Toxon.Integresql.Client.Models;

namespace Toxon.Integresql.Client;

public class DisposableDatabase : IAsyncDisposable
{
    private readonly IIntegresqlDatabaseClient _client;

    public DisposableDatabase(IIntegresqlDatabaseClient client, TestDatabase database)
    {
        _client = client;
        Database = database;
    }

    public TestDatabase Database { get; }
    
    public string GetConnectionString() => Database.GetConnectionString();

    public async ValueTask DisposeAsync() => await _client.ReturnDatabase(Database).ConfigureAwait(false);
}