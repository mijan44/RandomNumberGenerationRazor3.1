using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Random_Object_Generation.Data
{
    public abstract class RandomObject
    {
        public int Count;
        private bool isEnable;
        public bool IsEnable { get { return isEnable; } set { isEnable = value; if (value) totalEnabled++; else totalEnabled--; } }
        private int preferredPercentage; 
        public int PreferredPercentage { get { return preferredPercentage; }set { if (value > 100) preferredPercentage = 100; else if (value < 0) preferredPercentage = 0; else preferredPercentage = value; } }

        private static int totalEnabled=0;
        private static int TotalRandomObjects=0;
        private static int TotalGenerated=0;

        public string LastGeneratedObject;
        public abstract bool GenerateNext();

        public string Type = string.Empty;
        public RandomObject()
        {
            Count = 0;
            IsEnable = true;
            this.PreferredPercentage = 0;
            LastGeneratedObject = null;
            TotalRandomObjects++;
        }
        protected bool HasCrossedLimit()
        {
            double currentPercentage = GetCurrentPercentage();
            return currentPercentage > PreferredPercentage;
        }

        protected void Icrement()
        {
            Count++;
            TotalGenerated++;
        }
        public static int GetTotalObjectGeneratedCount()
        {
            return TotalGenerated;
        }
        public int GetThisObjectGeneratedCount()
        {
            return Count;
        }

        public double GetCurrentPercentage()
        {
            return TotalGenerated==0? 0: Count * 100.0 / (TotalGenerated);
        }
        public static int GetTotalEnabled()
        {
            return totalEnabled;
        }
        public static void SetTotalEnabled(int value)
        {
           totalEnabled=value;
        }

        public static void Reset()
        {
            TotalGenerated = 0;
        }
    }
}
