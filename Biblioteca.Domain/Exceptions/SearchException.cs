namespace BibliotecaNET8.Domain.Exceptions;

public class SearchException : Exception
{
    public SearchException(string message)
        : base(message)
    {
    }

    public SearchException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}