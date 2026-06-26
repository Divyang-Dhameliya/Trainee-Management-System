public class TransientException : Exception 
{
    public TransientException(string message, Exception? inner = null) : base(message, inner) {}
}

public class PermenentException : Exception 
{
    public PermenentException(string message, Exception? inner = null) : base(message, inner) {}
}