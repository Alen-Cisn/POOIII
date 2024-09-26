using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDecorator
{
    public abstract class BaseDecorator<T> : Decorable<T>
    {
        protected Decorable<T> componente;

        public BaseDecorator(Decorable<T> componente)
        {
            this.componente = componente;
        }

        public virtual string Process(string input)
        {
            return componente.Process(input);
        }
    }
}
