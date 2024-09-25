
namespace Patrones;

public interface IIterable<T> {
    public T Value { get; }
    public bool NextValue();
}