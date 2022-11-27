using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Random_Object_Generation.Data
{
    public class FloatRandomObject : RandomObject
    {

        public FloatRandomObject()
        {
            Type = "Float";
        }
        public override bool GenerateNext()
        {
            if (!HasCrossedLimit())
            {
                Icrement();
                LastGeneratedObject = (new Random().NextDouble() * Math.Pow(10, new Random().Next(10))).ToString();
                return true;
            }
            else
                return false;
        }
    }
}
