using DataProcessingService_Task1Radency_.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Interfaces
{
    // Інтерфейс від якого успадковуються класи, для зчитування файлів
    // НА МОЮ ДУМКУ БІЛЬШ ДОЦІЛЬНО БУДЕ ПЕРЕРОБИТИ НА БАЗОВИЙ КАЛС, ТА ЗА ПОТРЕБИ (ЯКЩО ФАЙЛИ МАТИМУТЬ ЯКІСЬ
    // ОСОБЛИВОСТІ ЧИТАННЯ), ТО УСПОДКОВУВАТИ ЙОГО, ТА ПЕРЕВИЗНАЧАТИ МЕТОДИ.

    internal interface IReaderFiles
    {
        void ReadFile(string pathFile);

        bool StringCheck(string str);

        void LineProcessing(string str);

        void SeveJSON(List<City> cities);
    }
}
