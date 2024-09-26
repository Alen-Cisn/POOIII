using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorImageProcesor
{
    public interface ImageProcessor
    {
        void Process(string path, string savePath);
    }
}
