using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using DataProcessingService_Task1Radency_.Classes;

namespace DataProcessingService_Task1Radency_
{
    internal class Program
    {
        static void Main()
        {
            string status = "";



        //Блок коду з міткою Reset, до якого можна буде звернутися за допомогою оператора goto
        Reset:
            {
                //Встановлюємо таймер, який буде генерувати файл meta.log, а також буде створювати нові папки з датами
                CreatorDirectories.SetupTimer();

                //Запускаємо стеження за двома видами файлів txt та csv
                Console.WriteLine("Program start:");
                FileWatcher fw1 = new FileWatcher();
                FileWatcher fw2 = new FileWatcher();
                fw1.LookFile("*.txt");
                fw2.LookFile("*.csv");

                status = "";
            }



            //Лічильник, який відповідає за вивід повідомлення: "Invalid command.".
            int countIncComand = 0;
            //Цикл зчитування, поки не буде введено вірну команду
            do
            {
                if (countIncComand > 0)
                {
                    Console.WriteLine("Invalid command.");
                }

                //Зчитування статусних кодів reset та stop
                status = Console.ReadLine();
                countIncComand++;
            }
            while (status != "reset" && status != "stop");
            countIncComand = 0;
            //При команді reset спрацьовує оператор goto, який вертає на те місце, де є та мітка,
            //яка зазначена після оператора.
            if (status == "reset")
            {
                goto Reset;
            }
            //При команді stop застосунок зупиняється 
            else if (status == "stop")
            {
                return;
            }
        }      
    }
}