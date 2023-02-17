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

            FileWatcher fw1 = new FileWatcher();
            FileWatcher fw2 = new FileWatcher();

            fw1.LookFile("*.txt");
            fw2.LookFile("*.csv");
            

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        
    }
}