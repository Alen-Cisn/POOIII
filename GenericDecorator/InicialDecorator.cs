namespace GenericDecorator
{
    public class InicialDecorator<T> : BaseDecorator<T>
    {
        public InicialDecorator(Decorable<T> componente) : base(componente)
        {
        }

        public override string Process(string input)
        {
            return "[Inicio] - " + base.Process(input);
        }
    }
}
