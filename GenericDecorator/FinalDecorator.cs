namespace GenericDecorator
{
    public class FinalDecorator<T> : BaseDecorator<T>
    {
        public FinalDecorator(Decorable<T> componente) : base(componente)
        {
        }

        public override string Process(string input)
        {
            return base.Process(input) + " - [Fin]";
        }
    }
}
