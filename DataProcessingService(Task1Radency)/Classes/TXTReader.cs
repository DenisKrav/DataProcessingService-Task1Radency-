using CsvHelper;
using DataProcessingService_Task1Radency_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Classes
{
    internal class TXTReader : IReaderFiles
    {
        public async void /*Task<string>*/ ReadFile(string pathFile)
        {
            string data = "";

            using (StreamReader reader = new StreamReader(pathFile))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
