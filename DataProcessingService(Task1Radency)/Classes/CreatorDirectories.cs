using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DataProcessingService_Task1Radency_.Classes
{
    internal static class CreatorDirectories
    {
        private static string folderPath = GetDirectory();

        public static string FolderPath { get { return folderPath; } }

        //Метод для отримання дерикторії
        public static string GetDirectory()
        {
            // Отримуємо поточну дату
            DateTime currentDate = DateTime.Now;

            // Створюємо рядок з назвою папки на основі поточної дати
            string folderName = currentDate.ToString("yyyy-MM-dd");

            // Створюємо шлях до папки, використовуючи метод Path.Combine
            folderPath = Path.Combine(ConfigurationManager.AppSettings.Get("FolderBPath"), folderName);

            // Перевіряємо, чи папка існує, і створюємо її, якщо потрібно
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Повертаємо шлях до папки
            return folderPath;
        }



        //Метод таймер, який спрацьовує опівночі та створює нову дерикторїю
        public static void SetupTimer()
        {
            // Створюємо таймер
            System.Timers.Timer timer = new System.Timers.Timer();

            // Встановлюємо час, коли таймер повинен спрацювати
            DateTime targetTime;

            // Якщо поточний час дорівнює опівночі, то встановлюємо таймер на опівніч наступного дня
            if (DateTime.Now.TimeOfDay == TimeSpan.Zero)
            {
                targetTime = DateTime.Today.AddDays(1);
            }
            // Інакше встановлюємо таймер на опівніч цього дня, точніше на 23:59:59
            else
            {
                targetTime = DateTime.Today.AddDays(1).AddSeconds(-1);
            }

            TimeSpan timeUntilTarget = targetTime - DateTime.Now;

            // Встановлюємо інтервал таймера
            timer.Interval = timeUntilTarget.TotalMilliseconds;

            // Встановлюємо обробник події для таймера
            timer.Elapsed += Timer_Elapsed;

            // Запускаємо таймер
            timer.Start();
        }



        //Метод, який спрацьовую при спрацюванні таймера, та запускає його знову.
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Записуємо дані у мета файл
            MetaFileData.CreateMetaFileData(folderPath);

            // Встановлюємо новий шлях до папки з поточною датою
            folderPath = GetDirectory();

            // Встановлюємо таймер для наступної операції створення папки
            SetupTimer();
        }
    }
}
