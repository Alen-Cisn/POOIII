using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDecorator
{
    public class ConcreteComponent<T> : Decorable<T>
    {
        public string Process(string cadena)
        {
            return cadena;
        }
    }
}
