namespace Txt.Shared.Result;

public class OneOf<T1, T2>
{
    private readonly object _value;
    private readonly bool _isT1;

    public OneOf(T1 value)
    {
        _value = value!;
        _isT1 = true;
    }
    public OneOf(T2 value)
    {
        _value = value!;
        _isT1 = false;
    }

    /// <summary>
    /// Executes one of the provided functions based on the underlying type of the value stored in the OneOf instance.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="caseT1">Function to execute if the stored value is of type T1.</param>
    /// <param name="caseT2">Function to execute if the stored value is of type T2.</param>
    /// <returns>The result of the executed function.</returns>
    public TResult Match<TResult>(Func<T1, TResult> caseT1, Func<T2, TResult> caseT2)
        => _isT1 switch
        {
            // Invoke caseT1 if the value is of type T1
            true => caseT1((T1)_value),
            // Invoke caseT2 if the value is of type T2
            false => caseT2((T2)_value),
        };
}