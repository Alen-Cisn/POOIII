using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorImageProcesor
{
    public class GrayscaleDecorator : ImageProcessorDecorator
    {
        public GrayscaleDecorator(ImageProcessor processor) : base(processor)
        {

        }

        public override void Process(string path, string savePath)
        {

            try
            {
                using (Image image = Image.FromFile(path))
                {
                    Bitmap grayImg = new Bitmap(image.Width, image.Height);
                    for (int y = 0; y < image.Height; y++)
                    {
                        for (int x = 0; x < image.Width; x++)
                        {
                            Color originalColor = ((Bitmap)image).GetPixel(x, y);
                            int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));

                            Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                            grayImg.SetPixel(x, y, grayColor);
                        }
                    }

                    grayImg.Save(savePath);
                    Console.WriteLine("Filtro de escala de grises aplicado");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.ToString());
            }
        }
    }
}
