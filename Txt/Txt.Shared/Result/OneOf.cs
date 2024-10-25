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

    public TResult Match<TResult>(Func<T1, TResult> caseT1, Func<T2, TResult> caseT2)
        => _isT1 switch
        {
            true => caseT1((T1)_value),
            false => caseT2((T2)_value),
        };
}