using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataProcessingService_Task1Radency_.Classes
{
    //Клас для зберігання даних для файлу meta.log
    internal static class MetaFileData
    {
        //Змінна для кількості проаналізованих даних
        private static int parsed_files = 0;
        //Змінна для кількості проаналізованих рядків
        private static int parsed_lines = 0;
        //Змінна для кількості знайдених помилок
        private static int found_errors = 0;
        //Змінна для шляхів файлів, де були знайдені помилки
        private static List<string> invalid_files = new List<string>();



        //Властивості, які доступні для читання
        public static int Parsed_files { get => parsed_files; }
        public static int Parsed_lines { get => parsed_lines; }
        public static int Found_errors { get => found_errors; }
        public static List<string> Invalid_files { get => invalid_files; }



        //Методи для додання даних
        public static void AddParsed_files()
        {
            parsed_files++;
        }
        public static void AddParsed_lines()
        {
            parsed_lines++;
        }
        public static void AddFound_errors()
        {
            found_errors++;
        }
        public static void AddInvalid_files(string pathInvalideFile)
        {
            invalid_files.Add(pathInvalideFile);
        }



        //Метод для видалення усх даних
        private static void ClearOll()
        {
            parsed_files = 0;
            parsed_lines = 0;
            found_errors = 0;
            invalid_files.Clear();
        }



        public static async void CreateMetaFileData(string path)
        {
            // Дані, які будуть записуватися
            string text = "parsed_files: " + parsed_files + "\n";
            text += "parsed_lines: " + parsed_lines + "\n";
            text += "found_errors: " + found_errors + "\n";
            text += "invalid_files: " + found_errors + "\n";
            foreach (var pathFile in invalid_files) 
            { 
                text += pathFile + "\n";
            }

            // Запис файла у файл meta.log
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }

            ClearOll();
        }
    }
}
