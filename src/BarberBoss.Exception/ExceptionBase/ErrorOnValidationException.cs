using System.Net;

namespace BarberBoss.Exception.ExceptionBase;

public class ErrorOnValidationException(List<string> errorMessages) : BarberBossException(string.Empty)
{
    private readonly List<string> _errors = errorMessages;

    public override int StatusCode => (int)HttpStatusCode.BadGateway;

    public override List<string> GetErrors() => _errors;
}
