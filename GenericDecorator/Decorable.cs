using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDecorator
{
    public interface Decorable<T>
    {
        string Process(string cadena);
    }
}
