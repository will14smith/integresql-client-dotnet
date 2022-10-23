using System.Threading.Tasks;
using Toxon.Integresql.Client.Models;

namespace Toxon.Integresql.Client;

public interface IIntegresqlTemplateClient
{
    Task<TemplateDatabase> InitializeTemplate(string hash);
    Task FinalizeTemplate(TemplateDatabase database);
    Task DiscardTemplate(TemplateDatabase database);
}