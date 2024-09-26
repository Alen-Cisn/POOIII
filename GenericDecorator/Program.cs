namespace GenericDecorator
{
    public class Program
    {
        static void Main()
        {
            Decorable<string> componente = new ConcreteComponent<string>();

            componente = new InicialDecorator<string>(componente);
            //componente = new FinalDecorator<string>(componente);

            string input = "Cadena de texto de ejemplo";

            Console.WriteLine(componente.Process(input));
        }
    }
}