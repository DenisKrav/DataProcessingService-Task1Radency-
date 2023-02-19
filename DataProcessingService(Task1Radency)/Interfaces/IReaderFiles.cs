using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Interfaces
{
    internal interface IReaderFiles
    {
        void /*Task<string>*/ ReadFile(string pathFile);
    }
}
