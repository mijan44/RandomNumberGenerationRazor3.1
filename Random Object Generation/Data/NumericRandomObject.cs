using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Random_Object_Generation.Data
{
    public class NumericRandomObject : RandomObject
    {

        public NumericRandomObject()
        {
            Type = "Numeric";
        }
        public override bool GenerateNext()
        {
            if (!HasCrossedLimit())
            {
                Icrement();
                LastGeneratedObject = new Random().Next().ToString();
                return true;
            }
            else
                return false;
        }
    }
}
