using ConsoleApp1;
using System.Net;

namespace TestTaskDotNET
{
    /// <summary>
    /// Класс для работы с файлом.
    /// </summary>
    internal class WorkWithFiles
    {
        /// <summary>
        /// Считывает текст из файла.
        /// </summary>
        /// <param name="pathToRead">Путь к файлу.</param>
        /// <returns>Массив строк файла.</returns>
        internal string[] ReadFromFile(string pathToRead) 
        {
            if (File.Exists(pathToRead))
            {
                var textFromFile = File.ReadAllLines(pathToRead);
                return textFromFile;
            }

            throw new Exception($"Файл по данному пути \"{pathToRead}\" не существует.");
        }

        
        /// <summary>
        /// Обрабатывает текст файла по заданным параметрам.
        /// </summary>
        /// <param name="rows">Массив строк файла.</param>
        /// <param name="addressStart">Нижняя грацина Ip из параметра.</param>
        /// <param name="addressMask">Маска из параметров.</param>
        /// <param name="timeStart">Нижняя граница времени из параметров.</param>
        /// <param name="timeEnd">Верхняя граница времени из параметров.</param>
        /// <returns>Содержимое обработанного файла.</returns>
        internal string SortText(string[] rows, IPAddress addressStart,IPAddress addressMask, DateTime timeStart, DateTime timeEnd)
        {
            var sortedText = new Dictionary<IPAddress,int>();

            foreach (var line in rows)
            {
                var stringIp = GetIpFromTextLine(line);
                var ipFromFile = IPAddress.Parse(stringIp);

                var date = DateTime.Parse(line.Substring(line.IndexOf(':') + 1));

                var dateIsRange = IsDateInRange(date, timeStart, timeEnd);
                var ipIsRange = IpHelper.CheckRangeIp(addressStart, addressMask, ipFromFile);

                if (dateIsRange && ipIsRange)
                {
                    IpHelper.AddIpInDictionary(sortedText, ipFromFile);
                }
            }
             
            return string.Join(Environment.NewLine, sortedText.Select(x => $"{x.Key} - обращений: {x.Value}")); ;
        }

        /// <summary>
        /// Получает текстовое представление Ip-адреса из строки файла.
        /// </summary>
        /// <param name="line">Строка файла.</param>
        /// <returns>Текстовое представление Ip.</returns>
        private string GetIpFromTextLine(string line)
        {
            return line.Substring(0, line.IndexOf(':'));
        }
        
        /// <summary>
        /// Сохранение текста в файл.
        /// </summary>
        /// <param name="sortedText">Сохраняемый текст.</param>
        /// <param name="pathToSave">Путь к файлу.</param>
        internal void SaveText(string sortedText, string pathToSave)
        {
            File.WriteAllText(pathToSave, sortedText);
        }

        /// <summary>
        /// Проверяет входит ли дата обращения Ip в необходимый диапазон.
        /// </summary>
        /// <param name="timeFile">Время обращения из текста.</param>
        /// <param name="timeStart">Начальная граница времени.</param>
        /// <param name="timeEnd">Конечная граница времени.</param>
        /// <returns>Входит ли дата в диапазон.</returns>
        private bool IsDateInRange(DateTime timeFile, DateTime timeStart, DateTime timeEnd)
        {
            if (timeFile >= timeStart && timeFile <= timeEnd)
            {
                return true;
            }
            
            return false;
        }
    }
}
