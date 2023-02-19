using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DataProcessingService_Task1Radency_.Classes
{
    internal class FileWatcher
    {
        //Створення та ініціалізація об'єкту класу FileSystemWatcher, який відповідає за відстеження за файлами
        //одного певного типу, у заданій папці
        private FileSystemWatcher watcher = new FileSystemWatcher(ConfigurationManager.AppSettings.Get("FolderAPath"));

        public void LookFile(string filterFile)
        {
            try
            {
                //Задаємо, які саме зміни будуть відстежуватись у файлі або папці
                watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

                //Підписуємося на події, які виникають при ти чи інших подіях, наприклад при видалені файлу, доданні,
                //перейменуванні і так далі, а також присвоюємо їм метди, які будуть виконуватися при цих подіях.
                watcher.Created += OnCreated;
                watcher.Error += OnError;

                //ЗАдаємо, який саме тип файлу буде відстежуватись
                watcher.Filter = filterFile;
                watcher.IncludeSubdirectories = true;
                //Задаємо для об'єкта класу FileSystemWatcher, що треба відстежувати компонент за вказаним шляхом,
                //якщо цього не вказати або або поставити false, то відстеження за вказаним шляхом не буде.
                watcher.EnableRaisingEvents = true;
            }
            //Обробка помилок які виникають, якщо файл конфігурації порожній або не доступний
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }



        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            // Створюємо таймер
            System.Timers.Timer timer = new System.Timers.Timer();

            // Встановлюємо час, коли таймер повинен спрацювати
            DateTime targetTime;
            //Якщо поточний час дорівнює опівночі, то встановлюємо таймер на опівніч наступного дня, тому додаэмо день
            if (DateTime.Now.TimeOfDay == TimeSpan.Zero)
            {
                targetTime = DateTime.Today.AddDays(1);
            }
            //Інакше встановлюємо таймер на опівніч цього дня, і тому к поточній даті додаємо 24, щоб вийшла опівніч цього дня
            else
            {
                targetTime = DateTime.Today.AddHours(24);
            }

            TimeSpan timeUntilTarget = targetTime - DateTime.Now;

            // Встановлюємо інтервал таймера
            timer.Interval = timeUntilTarget.TotalMilliseconds;


            // Встановлюємо обробник події для таймера
            timer.Elapsed += Timer_Elapsed;

            // Запускаємо таймер
            timer.Start();


        }

        static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Таймер спрацював о 24:00!");
        }




        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

    }
}
