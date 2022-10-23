namespace Toxon.Integresql.Client.Exceptions;

public class TemplateAlreadyInitializedException : IntegresqlException
{
    public TemplateAlreadyInitializedException() : base("template is already initialized") { }
}