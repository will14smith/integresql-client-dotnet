using System;
using System.Threading.Tasks;
using Toxon.Integresql.Client.Exceptions;
using Toxon.Integresql.Client.Models;

namespace Toxon.Integresql.Client;

public static class ClientExtensions
{
    public static async Task SetupTemplate(this IIntegresqlTemplateClient client, string hash, Func<string, Task> setupWithConnectionStringAction) => 
        await client.SetupTemplate(hash, config => setupWithConnectionStringAction(config.GetConnectionString())).ConfigureAwait(false);
    public static async Task SetupTemplate(this IIntegresqlTemplateClient client, string hash, Func<DatabaseConfig, Task> setupAction)
    {
        try
        {
            var database = await client.InitializeTemplate(hash).ConfigureAwait(false);
            await setupAction(database.Config).ConfigureAwait(false);
            await client.FinalizeTemplate(database).ConfigureAwait(false);
        }
        catch (TemplateAlreadyInitializedException)
        {
        }
    }
    
    public static async Task<DisposableDatabase> AcquireDatabase(this IIntegresqlDatabaseClient client, string hash)
    {
        var database = await client.GetDatabase(hash).ConfigureAwait(false);

        return new DisposableDatabase(client, database);
    }
}