using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTLab7
{

    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public class AdditionalData
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public Color Color { get; set; }

        public AdditionalData() {
            var rand = new Random();
            this.Number1 = rand.Next(1, 11);
            this.Number2 = rand.Next(1, 11);
            this.Color = (Color)rand.Next(0, 3);
        }
    }
}
