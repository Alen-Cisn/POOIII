
namespace Patrones;



public interface IIterable<T>
{
    IIterator<T> GetIterator();
}

public interface IIterator<T>
{
    public T Value { get; }
    public bool NextValue();
}
