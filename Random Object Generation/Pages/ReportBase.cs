using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Random_Object_Generation.Pages
{
    public enum ReadType
    {
        Numeric,
        Alphanumeric,
        Float
    }
    public class UnknownObject
    {

        private ReadType type;
        public ReadType Type { get { return type; } }
        private string value;
        public string Value { get { return value; } }
        public UnknownObject(string value)
        {
            this.value = value;
            DetectType();
        }

        private void DetectType()
        {
            Int64 numeric;
            double floatValuea;
            if (Int64.TryParse(Value, out numeric))
                type = ReadType.Numeric;
            else if (Double.TryParse(Value, out floatValuea))
                type = ReadType.Float;

            else
                type = ReadType.Alphanumeric;
                
        }
    }
    public class ReportBase:ComponentBase
    {
        public double NumericPercentage;
        public double AlphaNumbericPercentage;
        public double FloatPercentage;

        private string InFileName = "randomObject.txt";


        public IEnumerable<UnknownObject> unknownObjects;
        public ReportBase()
        {
            AlphaNumbericPercentage = 0;
            NumericPercentage = 0;
            FloatPercentage = 0;
        }
        protected override async Task OnInitializedAsync()
        {
            await Task.Run(IntializePercentage);
        }

        private void IntializePercentage()
        {
            List<UnknownObject> _unknownObjects = new List<UnknownObject>();
            int maxShow = 20;

            if (!File.Exists(InFileName))
            {
                unknownObjects = _unknownObjects;
                return;
            }
            try
            {
                using (var f = new StreamReader(InFileName))
                {
                    string line = string.Empty;
                    while ((line = f.ReadLine()) != null)
                    {
                        var parts = line.Split(',');

                        foreach(var value in parts)
                        {
                            UnknownObject ob = new UnknownObject(value);
                            if (_unknownObjects.Count() < maxShow)
                                _unknownObjects.Add(ob);
                            switch (ob.Type)
                            {
                                case ReadType.Numeric:
                                    NumericPercentage++;
                                    break;
                                case ReadType.Float:
                                    FloatPercentage++;
                                    break;
                                case ReadType.Alphanumeric:
                                    AlphaNumbericPercentage++;
                                    break;
                            }
                        }
                        
                    }
                }

            }
            catch
            {

            }
            unknownObjects = _unknownObjects;
            double total = AlphaNumbericPercentage + NumericPercentage + FloatPercentage;

            if (total != 0)
            {
                AlphaNumbericPercentage = AlphaNumbericPercentage*100 / total;
                FloatPercentage = FloatPercentage * 100 / total;
                NumericPercentage = NumericPercentage * 100 / total;
            }

        }
    }
}
