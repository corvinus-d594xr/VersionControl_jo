using _7gyak_D594XR.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7gyak_D594XR.Entities
{
    public class CarFactory : IToyFactory
    {
        public Toy CreateNew()
        {
            Car car = new Car();
            return car;
        }
    }
}
