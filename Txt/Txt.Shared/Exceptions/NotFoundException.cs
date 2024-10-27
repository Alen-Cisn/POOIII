
namespace Txt.Shared.Exceptions;

[Serializable]
public class NotFoundExceptionException : Exception
{
    public NotFoundExceptionException() { }
    public NotFoundExceptionException(string message) : base(message) { }
    public NotFoundExceptionException(string message, Exception inner) : base(message, inner) { }
    protected NotFoundExceptionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    { }
}