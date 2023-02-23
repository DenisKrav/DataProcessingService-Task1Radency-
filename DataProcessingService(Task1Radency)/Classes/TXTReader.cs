using CsvHelper;
using DataProcessingService_Task1Radency_.Classes.Models;
using DataProcessingService_Task1Radency_.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DataProcessingService_Task1Radency_.Classes
{
    internal class TXTReader : IReaderFiles
    {
        // Текстові файли, які оброблює клас повинні мати стандартне кодування UTF-8

        private List<City> cities = new List<City>();

        public async void ReadFile(string pathFile)
        {
            int countErrors = 0;

            // Відловлюємо помилки, які виникають, коли намагаємся зчитати файл, який використовується іншим процесом.
            // може виникнути при копіюванні файл і вставлянні його в ту ж саму папку без зміни.
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
                            // Оброблюємо рядок
                            LineProcessing(line);

                            // Збільшуємо лічильник перевірених рядків
                            MetaFileData.AddParsed_lines();
                        }
                        else
                        {
                            countErrors++;

                            // Якщо помилка зустрілася перший раз, то ми заносимо щлях файлу до списку файлів з
                            // помилками, але лише один раз, для цього потрібен лічильник, якщо помилки зустрічаються
                            // і надалі, то ми більше не заносимо шлях, а просто рахуємо далі, скільки рядків з помилками
                            // і скільки було проаналізовано рядків.
                            if (countErrors == 1)
                            {
                                MetaFileData.AddInvalid_files(pathFile);
                            }

                            // Збільшуємо лічильник перевірених рядків та знайдених помилок
                            MetaFileData.AddParsed_lines();
                            MetaFileData.AddFound_errors();
                        }
                    }
                }

                // Зберігаємо файл у форматі JSON
                SeveJSON(cities);
            }
            catch(System.IO.IOException ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Метод перевірки валідності рядка
        public bool StringCheck(string str)
        {
            // Регулярний вираз, який перевіряє рядки на правильність
            string pattern = @"^[A-Za-z]+,\s[A-Za-z]+,\s“[A-Za-z,\s\d]+”,\s\d+\.\d+,\s\d{4}-\d{2}-\d{2},\s\d+,\s[A-Za-z]+$";
            // Зіставляємо рядок з регулярним виразом
            Match match = Regex.Match(str, pattern);

            // Вертаємо результат порівняння
            return match.Success;
        }

        // Метод для обробки рядка
        public void LineProcessing(string str)
        {
            string[] words = str.Split(new char[] { ' ', ',', '“', '”', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Заносимо дані про платника
            // Перетворюємо число заплаченої суми та дату з урахуванням культурних налаштувань
            Payer payer = new Payer(words[0] + " " + words[1], Double.Parse(words[6], CultureInfo.InvariantCulture), DateTime.ParseExact(words[7], "yyyy-dd-MM", CultureInfo.InvariantCulture), Convert.ToInt32(words[8]));

            // Перевіряємо список міст, якщо немає міста, то додаємо одразу усю інформацію,
            // так само із сервісами, якщо не має додаємо, одразу з усією інфою, інакше додаємо лише платника
            foreach (City citi in cities)          
            {                    
                if (citi.Name == words[2])                    
                {                      
                    foreach (Service service in citi.Services)                     
                    {                           
                        if (service.Name == words[9])                          
                        {
                            service.Payers.Add(payer);
                            
                            service.Total++;
                            citi.Total += payer.Payment;
                        }                      
                    }
                        
                    if (!citi.Services.Any() || !citi.Services.Any(n => n.Name == words[9]))
                    {                         
                        citi.Services.Add(new Service(words[9], new List<Payer>() { payer }));

                        citi.Services[citi.Services.FindIndex(n => n.Name == words[9])].Total += payer.Payment;
                        cities[cities.FindIndex(n => n.Name == words[2])].Total += payer.Payment;
                    }                    
                }              
            }               
            if (!cities.Any() || !cities.Any(n => n.Name == words[2]))                
            {                 
                cities.Add(new City(words[2], new List<Service>() { new Service(words[9], new List<Payer>() { payer }) }));

                cities[cities.FindIndex(n => n.Name == words[2])].Services[0].Total += payer.Payment;
                cities[cities.FindIndex(n => n.Name == words[2])].Total += payer.Payment;
            }
        }

        // Метд для збереження JSON файлу
        public void SeveJSON(List<City> cities)
        {
            // Формує рядок формту JSON
            string json = JsonConvert.SerializeObject(cities, Formatting.Indented);
            // Записуємо у файл
            File.WriteAllTextAsync(/*ConfigurationManager.AppSettings.Get("FolderBPath")*/CreatorDirectories.FolderPath + $"/output{MetaFileData.Parsed_files}.json", json);
            // Очищуємо масив cities
            cities.Clear();
        }
    }
}
