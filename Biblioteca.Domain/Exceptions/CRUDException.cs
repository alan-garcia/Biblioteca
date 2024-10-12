namespace BibliotecaNET8.Domain.Exceptions;

public class CRUDException : Exception
{
    public CRUDException(string message)
        : base(message)
    {
    }

    public CRUDException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}