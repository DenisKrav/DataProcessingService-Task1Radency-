using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DataProcessingService_Task1Radency_.Services
{
    internal static class CreatorDirectories
    {
        private static string folderPath = GetDirectory();

        public static string FolderPath { get { return folderPath; } }

        //Метод для отримання дерикторії
        public static string GetDirectory()
        {
            DateTime currentDate = DateTime.Now;
            string folderName = currentDate.ToString("yyyy-MM-dd");
            folderPath = Path.Combine(ConfigurationManager.AppSettings.Get("FolderBPath"), folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        //Метод таймер, який спрацьовує опівночі та створює нову дерикторїю
        public static void SetupTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            DateTime targetTime;

            if (DateTime.Now.TimeOfDay == TimeSpan.Zero)
            {
                targetTime = DateTime.Today.AddDays(1);
            }
            else
            {
                targetTime = DateTime.Today.AddDays(1).AddSeconds(-1);
            }

            TimeSpan timeUntilTarget = targetTime - DateTime.Now;
            timer.Interval = timeUntilTarget.TotalMilliseconds;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        //Метод, який спрацьовую при спрацюванні таймера, та запускає його знову.
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MetaFileData.CreateMetaFileData(folderPath);
            folderPath = GetDirectory();
            SetupTimer();
        }
    }
}
