using System.Threading.Tasks;
using Toxon.Integresql.Client.Models;

namespace Toxon.Integresql.Client;

public interface IIntegresqlDatabaseClient
{
    Task<TestDatabase> GetDatabase(string hash);
    Task ReturnDatabase(TestDatabase database);
}