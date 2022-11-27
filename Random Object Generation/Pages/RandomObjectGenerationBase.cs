using Microsoft.AspNetCore.Http;
using Random_Object_Generation.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Random_Object_Generation.Pages
{
    public class RandomObjectGenerationBase: Microsoft.AspNetCore.Components.ComponentBase
    {
        public IEnumerable<RandomObject> randomObjects;
        private bool Generate=false;
        private int RandomObjectCount = 3;
        private long fileSizeInKB = 1;
        public long FileSizeInKB
        {
            get
            {
                return fileSizeInKB;
            }
            set
            {
                fileSizeInKB = value < 1 ? 1 : value;
            }
        }
        private string OutFileName = "randomObject.txt";
        public bool AtLeastOneEnabled = true;

        private int totalPercentage = 0;
        public int TotalPercentage { get { return totalPercentage; } }
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public HttpContext Context => _httpContextAccessor.HttpContext;

        //public RandomObjectGenerationBase(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(IntializeRandomObject);
            Reset();
            ReCalculatePercentage();
        }

        private void Reset()
        {
            int totalEnabled = 0;
            for (int i = 0; i < randomObjects.Count(); i++)
            {
                randomObjects.ElementAt(i).Count = 0;
                if (randomObjects.ElementAt(i).IsEnable)
                    totalEnabled++;
            }
            RandomObject.SetTotalEnabled(totalEnabled);
            RandomObject.Reset();

            if (File.Exists(OutFileName))
            {
                try
                {
                    File.Delete(OutFileName);
                }
                catch
                {

                }
            }

        }

        private void IntializeRandomObject()
        {
            randomObjects = new List<RandomObject>{
                new NumericRandomObject(),
                new AlphaNumericRandomObject(),
                new FloatRandomObject()
            };
        }

        public void StartGenerating()
        {
            if(!AtLeastOneEnabled || TotalPercentage!=100)
                return;

            Reset();

            Task.Run(async() =>
              {
                  Generate = true;
                  long currentFileSize = 0;
                  try
                  {
                      using (StreamWriter outFile = File.AppendText(OutFileName))
                      {
                          bool first = true;
                          while (Generate && currentFileSize < FileSizeInKB * 1024)
                          {
                              int rindex = new Random().Next(RandomObjectCount);
                              RandomObject ro = randomObjects.ElementAt(rindex);
                              if (!ro.IsEnable) continue;
                              await InvokeAsync(() =>
                                {
                                    if (ro.GenerateNext())
                                    {
                                        outFile.Write((first ? "" : ",") + ro.LastGeneratedObject);
                                        currentFileSize += 4;
                                        StateHasChanged();
                                        currentFileSize = new FileInfo(OutFileName).Length;
                                        first = false;
                                    }
                                });

                              //Thread.Sleep(100);
                          }

                      }
                  }
                  catch
                  {

                  }

                  Generate = false;

              });
        }
        public void StopGenerating()
        { 
            Generate = false;
        }
        
        public void CheckBoxClicked(RandomObject ro)
        {
            ro.IsEnable = !ro.IsEnable;
            if (!ro.IsEnable)
                ro.PreferredPercentage = 0;

            AtLeastOneEnabled = false;
            for(int i = 0; i < RandomObjectCount; i++)
            {
                AtLeastOneEnabled=AtLeastOneEnabled || randomObjects.ElementAt(i).IsEnable;

            }
            //StateHasChanged();
            ReCalculatePercentage();
        }

        private void ReCalculatePercentage()
        {
            if (RandomObject.GetTotalEnabled() != 0)
            {
                int averageDistribution = 100 / RandomObject.GetTotalEnabled();
                bool first = true;
                int leftOver = 100 - averageDistribution * RandomObject.GetTotalEnabled();
                for(int i = 0; i < randomObjects.Count(); i++)
                {
                    if (randomObjects.ElementAt(i).IsEnable)
                    {
                        randomObjects.ElementAt(i).PreferredPercentage = averageDistribution+(first?leftOver:0);
                        first = false;
                    }
                }

            }
            totalPercentage = 100;
        }

        public void PreferredPercentageChanged(RandomObject ro, Microsoft.AspNetCore.Components.ChangeEventArgs eventArgs)
        {
            totalPercentage = 0;
            ro.PreferredPercentage = Int32.Parse((string)eventArgs.Value);
            for (int i = 0; i < randomObjects.Count(); i++)
                totalPercentage += randomObjects.ElementAt(i).PreferredPercentage;
        }

        public bool IsGenerating => Generate;
    }
}
