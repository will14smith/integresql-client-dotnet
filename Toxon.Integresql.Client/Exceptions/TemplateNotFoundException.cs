namespace Toxon.Integresql.Client.Exceptions;

public class TemplateNotFoundException : IntegresqlException
{
    public TemplateNotFoundException() : base("template not found") { }
}