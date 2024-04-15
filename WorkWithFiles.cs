using ConsoleApp1;
using System.Net;

namespace TestTaskDotNET
{
    internal class WorkWithFiles
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToRead"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="text">Считываемый файл</param>
        /// <param name="addressStart">Нижняя грацина Ip из параметра</param>
        /// <param name="addressMask">Маска из параметров</param>
        /// <param name="timeStart">Нижняя граница времени из параметров</param>
        /// <param name="timeEnd">Верхняя граница времени из параметров</param>
        /// <returns>Строку, которая будет записана в файл</returns>
        internal string SortText(string[] text, IPAddress addressStart,IPAddress addressMask, DateTime timeStart, DateTime timeEnd)
        {
            var sortedText = new Dictionary<IPAddress,int>();

            foreach (var line in text)
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
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string GetIpFromTextLine(string line)
        {
            return line.Substring(0, line.IndexOf(':'));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortedText"></param>
        /// <param name="pathToSave"></param>
        internal void SaveText(string sortedText, string pathToSave)
        {
            File.WriteAllText(pathToSave, sortedText);
        }

        /// <summary>
        /// Проверка входит ли дата обращения Ip в необходимый диапазон
        /// </summary>
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
