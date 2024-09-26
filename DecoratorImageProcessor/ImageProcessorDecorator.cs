using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorImageProcesor
{
    public abstract class ImageProcessorDecorator : ImageProcessor
    {
        protected ImageProcessor processor;

        public ImageProcessorDecorator(ImageProcessor processor)
        {
            this.processor = processor;
        }

        public virtual void Process(string path, string savePath)
        {
            processor.Process(path, savePath);
        }
    }
}
