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

        //Метод для моніторингу файлів у папці
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


        // Метод, який відслідковує створення нових файлів
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            //////
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            /////

            string typeFile = e.FullPath.Substring(e.FullPath.LastIndexOf("."), 4);

            if (typeFile == ".txt")
            {
                TXTReader txtReader = new TXTReader();
                txtReader.ReadFile(e.FullPath);
            }
            if (typeFile == ".csv")
            {
                CSVReader csvReader = new CSVReader();
                csvReader.ReadFile(e.FullPath);
            }

            MetaFileData.AddParsed_files();
        }


    }
}
