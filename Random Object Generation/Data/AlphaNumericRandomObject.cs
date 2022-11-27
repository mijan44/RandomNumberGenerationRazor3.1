using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Random_Object_Generation.Data
{
    public class AlphaNumericRandomObject : RandomObject
    {

        public AlphaNumericRandomObject()
        {
            Type = "AlphaNumeric";
        }
        public override bool GenerateNext()
        {
            if (!HasCrossedLimit())
            {
                Icrement();
                LastGeneratedObject = GenerateRandomAlphaNumeric();
                return true;
            }
            else
                return false;

        }

        private string GenerateRandomAlphaNumeric()
        {
            string randomAlphaNumber = new string(' ', new Random().Next(1,11));
            int size = new Random().Next(1, 11); 
            for(int i = 0; i < 10; i++)
            {
                bool isAlpha = Convert.ToBoolean(new Random().Next(2));
                if (isAlpha)
                {
                    bool isCap = Convert.ToBoolean(new Random().Next(2));
                    int pos = new Random().Next(26);
                    randomAlphaNumber += (isCap) ? ((char)(pos + 65)).ToString() : ((char)(pos + 97)).ToString();
                }
                else
                    randomAlphaNumber += new Random().Next(10).ToString();
            }

            randomAlphaNumber += new string(' ',new Random().Next(1,11)); 
            return randomAlphaNumber;
        }
    }
}
