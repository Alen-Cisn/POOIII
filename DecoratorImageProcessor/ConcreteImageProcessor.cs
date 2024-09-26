using System.Drawing;

namespace DecoratorImageProcesor
{
    public class ConcreteImageProcessor : ImageProcessor
    {
        public void Process(string path, string savePath)
        {
            using (Image image = Image.FromFile(path))
            {
                Console.WriteLine("Imagen cargada");
                image.Save(savePath);
            }
        }
    }
}
