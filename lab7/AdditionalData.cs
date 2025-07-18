using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTLab7
{
    public enum Categories
    {
        CategoryA, CategoryB, CategoryC, CategoryD, CategoryE
    }

    public class AdditionalData : IComparable
    {
        public int Number { get; set; }
        public Categories Category { get; set; }


        public int CompareTo(object obj)
        {

            var obj2 = obj as AdditionalData;

            if (obj2 == null) return 1;

            return Number.CompareTo(obj2.Number);

        }

    }
}
