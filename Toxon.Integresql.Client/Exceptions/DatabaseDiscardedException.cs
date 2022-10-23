namespace Toxon.Integresql.Client.Exceptions;

public class DatabaseDiscardedException : IntegresqlException
{
    public DatabaseDiscardedException() : base("database was discarded (typically failed during initialize/finalize)") { }
}