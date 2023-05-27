using DataProcessingService_Task1Radency_.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Services
{
    // Текстові файли, які оброблює клас повинні мати стандартне кодування UTF-8
    public class FileReader
    {
        private List<City> cities = new List<City>();

        // Метод для зчитування файлів, який приймає шлях до файлу, а також приймає кількість помилок,
        // від яких починається запис у файл із статистикою за день
        public virtual async void ReadFile(string pathFile, int maxCountErrorsFile = 1)
        {
            int countErrors = 0;

            // Відловлюємо помилки, які виникають, коли намагаємся зчитати файл, який використовується іншим процесом.
            // може виникнути при копіюванні файла і вставлянні його в ту ж саму папку без зміни.
            // Можна буде вирішити за допомогою ШАБЛОНА ПОВТОРНОЇ СПРОБИ.
            try
            {
                using (StreamReader reader = new StreamReader(pathFile, detectEncodingFromByteOrderMarks: true))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (StringCheck(line))
                        {
                            LineProcessing(line);

                            MetaFileData.AddParsed_lines();
                        }
                        else
                        {
                            countErrors++;

                            // Якщо помилка зустрілася перший раз, то ми заносимо щлях файлу до списку файлів з
                            // помилками, але лише один раз, для цього потрібен лічильник, якщо помилки зустрічаються
                            // і надалі, то ми більше не заносимо шлях, а просто рахуємо далі, скільки рядків з помилками
                            // і скільки було проаналізовано рядків.
                            if (countErrors == maxCountErrorsFile)
                            {
                                MetaFileData.AddInvalid_files(pathFile);
                            }

                            if (countErrors >= maxCountErrorsFile)
                            {
                                MetaFileData.AddParsed_lines();
                                MetaFileData.AddFound_errors();
                            }                                
                        }
                    }
                }

                SeveJSON(cities);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Метод перевірки валідності рядка
        public bool StringCheck(string str)
        {
            string pattern = @"^[A-Za-z]+,\s[A-Za-z]+,\s“[A-Za-z,\s\d]+”,\s\d+\.\d+,\s\d{4}-\d{2}-\d{2},\s\d+,\s[A-Za-z]+$";
            Match match = Regex.Match(str, pattern);

            return match.Success;
        }

        // Метод для обробки рядка
        public void LineProcessing(string str)
        {
            string[] information = str.Split(new char[] { ' ', ',', '“', '”', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Payer payer = new Payer(information[0] + " " + information[1], Double.Parse(information[6], CultureInfo.InvariantCulture), DateTime.ParseExact(information[7], "yyyy-dd-MM", CultureInfo.InvariantCulture), Convert.ToInt32(information[8]));

            CheckAndAddCity(payer, information);
        }

        // Метд для збереження JSON файлу
        private void SeveJSON(List<City> cities)
        {
            string json = JsonConvert.SerializeObject(cities, Formatting.Indented);
            File.WriteAllTextAsync(CreatorDirectories.FolderPath + $"/output{MetaFileData.Parsed_files}.json", json);
            cities.Clear();
        }

        // Метод для перевірки наявності міста та занесення даних
        private void CheckAndAddCity(Payer payer, string[] information)
        {
            foreach (City city in cities)
            {
                if (city.Name == information[2])
                {
                    CheckAndAddService(payer, information, city);
                }
            }
            if (!cities.Any() || !cities.Any(n => n.Name == information[2]))
            {
                cities.Add(new City(information[2], new List<Service>() { new Service(information[9], new List<Payer>() { payer }) }));

                cities[cities.FindIndex(n => n.Name == information[2])].Services[0].Total += payer.Payment;
                cities[cities.FindIndex(n => n.Name == information[2])].Total += payer.Payment;
            }
        }

        // Метод для перевірки наявності сервіса та занесення даних
        private void CheckAndAddService(Payer payer, string[] information, City city)
        {
            foreach (Service service in city.Services)
            {
                if (service.Name == information[9])
                {
                    service.Payers.Add(payer);

                    service.Total++;
                    city.Total += payer.Payment;
                }
            }

            if (!city.Services.Any() || !city.Services.Any(n => n.Name == information[9]))
            {
                city.Services.Add(new Service(information[9], new List<Payer>() { payer }));

                city.Services[city.Services.FindIndex(n => n.Name == information[9])].Total += payer.Payment;
                cities[cities.FindIndex(n => n.Name == information[2])].Total += payer.Payment;
            }
        }
    }
}
