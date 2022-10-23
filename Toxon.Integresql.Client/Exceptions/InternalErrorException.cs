namespace Toxon.Integresql.Client.Exceptions;

public class InternalErrorException : IntegresqlException
{
    public InternalErrorException(string message) : base(message) { }
}