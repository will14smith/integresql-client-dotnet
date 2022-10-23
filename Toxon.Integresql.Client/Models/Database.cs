namespace Toxon.Integresql.Client.Models;

public abstract class Database
{
    protected Database(string hash, DatabaseConfig config)
    {
        Hash = hash;
        Config = config;
    }

    public string Hash { get; }
    public DatabaseConfig Config { get; }
}