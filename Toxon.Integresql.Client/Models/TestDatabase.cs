namespace Toxon.Integresql.Client.Models;

public class TestDatabase : Database
{
    public TestDatabase(int id, string hash, DatabaseConfig config) : base(hash, config)
    {
        Id = id;
    }
    
    public int Id { get; }
}