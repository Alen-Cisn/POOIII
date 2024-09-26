namespace DecoratorImageProcesor
{
    public class Program
    {
        static void Main()
        {
            //Crear procesador de imagen
            ImageProcessor processor = new ConcreteImageProcessor();

            //Aplicar decorator
            processor = new GrayscaleDecorator(processor);

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            //Procesar imagen
            //La imagen tiene que estar en la misma carpeta que la solucion de C#
            string path = Path.Combine(basePath, "..\\..\\..\\", "image.jpg");
            string savePath = Path.Combine(basePath, "..\\..\\..\\", "newImage.jpg");

            processor.Process(path, savePath);
        }
    }
}