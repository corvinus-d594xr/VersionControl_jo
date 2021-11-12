using _7gyak_D594XR.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7gyak_D594XR.Entities
{
    public class BallFactory : IToyFactory
    {
        public Color BallColor { get; private set; }

        public Toy CreateNew()
        {
            return new Ball(BallColor);
        }
    }
}
