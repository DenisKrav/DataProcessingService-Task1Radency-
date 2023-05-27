using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataProcessingService_Task1Radency_.Services
{
    //Клас для зберігання даних для файлу meta.log
    internal static class MetaFileData
    {
        private static int parsed_files = 0;
        private static int parsed_lines = 0;
        private static int found_errors = 0;
        private static List<string> invalid_files = new List<string>();

        public static int Parsed_files { get => parsed_files; }
        public static int Parsed_lines { get => parsed_lines; }
        public static int Found_errors { get => found_errors; }
        public static List<string> Invalid_files { get => invalid_files; }

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

        private static void ClearOll()
        {
            parsed_files = 0;
            parsed_lines = 0;
            found_errors = 0;
            invalid_files.Clear();
        }

        public static async void CreateMetaFileData(string path)
        {
            string text = "parsed_files: " + parsed_files + "\n";
            text += "parsed_lines: " + parsed_lines + "\n";
            text += "found_errors: " + found_errors + "\n";
            text += "invalid_files: " + found_errors + "\n";
            foreach (var pathFile in invalid_files)
            {
                text += pathFile + "\n";
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }

            ClearOll();
        }
    }
}
