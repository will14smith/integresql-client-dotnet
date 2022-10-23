using System.Text;

namespace Toxon.Integresql.Client.Models;

public static class DatabaseExtensions
{
    public static string GetConnectionString(this Database database) => database.Config.GetConnectionString();
    public static string GetConnectionString(this DatabaseConfig config)
    {
        var builder = new StringBuilder();

        builder.Append($"Host={config.Host};");
        builder.Append($"Port={config.Port};");
        if (!string.IsNullOrEmpty(config.Username)) { builder.Append($"Username={config.Username};"); }
        if (!string.IsNullOrEmpty(config.Password)) { builder.Append($"Password={config.Password};"); }
        builder.Append($"Database={config.Database};");

        foreach (var param in config.AdditionalParams)
        {
            builder.Append($"{param.Key}={param.Value};");
        }
        
        return builder.ToString();
    }
}