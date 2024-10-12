﻿namespace BibliotecaNET8.Infrastructure.Exceptions;

public class DatabaseConnectionException : Exception
{
    public DatabaseConnectionException(string message)
        : base(message)
    {
    }

    public DatabaseConnectionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}