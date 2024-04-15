using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1
{
    /// <summary>
    /// 
    /// </summary>
    internal static class IpHelper
    {
        /// <summary>
        /// Логическое "И" для Ip-адресса и маски
        /// </summary>
        internal static string CalculateLogicalAnd(int[] ipAddress, int[] mask)
        {
            var result = new int[4];
            for (var i = 0; i < 4; i++)
            {
                result[i] = ipAddress[i] & mask[i];
            }
            return string.Join(".", result);
        }

        /// <summary>
        /// Проверка есть ли в Dictionary найденный Ip, если нет - создает новую запись
        /// </summary>
        internal static void AddIpInDictionary(Dictionary<IPAddress, int> sortedDictionary, IPAddress ip)
        {
            if (!sortedDictionary.ContainsKey(ip))
            {
                sortedDictionary.Add(ip, 1);
            }
            else
            {
                int ipCount = sortedDictionary[ip] + 1;
                sortedDictionary[ip] = ipCount;
            }
        }

        /// <summary>
        /// Проверка входит ли Ip-адрес из файла
        /// в требуемый диапазон
        /// </summary>
        internal static bool CheckRangeIp(IPAddress addressStart, IPAddress addressMask, IPAddress logIp)
        {
            if (addressStart == null)
            {
                addressStart = IPAddress.Any;
            }

            var start = addressStart.ToString().Split(".").Select(x => int.Parse(x)).ToArray();
            var mask = addressMask.ToString().Split(".").Select(x => int.Parse(x)).ToArray();
            var log = logIp.ToString().Split(".").Select(x => int.Parse(x)).ToArray();

            var maskAndLog = CalculateLogicalAnd(mask, log);
            var maskAndStart = CalculateLogicalAnd(mask, start);

            return maskAndLog.Equals(maskAndStart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal static IPAddress? GetAddressStart(string ip)
        {
            if (ip == null)
            {
                return null;
            }

            if (IPAddress.TryParse(ip, out IPAddress? ipFromFile) == false)
            {
                throw new Exception("Ip-адрес введен неверно.");
            }

            if (ipFromFile.AddressFamily != AddressFamily.InterNetwork)
            {
                throw new Exception("Введен неправильный формат для ip-адреса.");
            }

            return ipFromFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="startIp"></param>
        /// <returns></returns>
        internal static IPAddress GetMaskAddress(string maskFromArgs, IPAddress startIp)
        {
            int numberForMask = 0;

            if (startIp == null && maskFromArgs != null)
            {
                return ConvertedMask(numberForMask);
            }

            if (!string.IsNullOrEmpty(maskFromArgs))
            {
                numberForMask = Convert.ToInt32(maskFromArgs);
            }

            if (numberForMask < 0 || numberForMask > 32)
            {
                throw new Exception("Введена некорректная маска.");
            }

            var mask = ConvertedMask(numberForMask);

            return mask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskInt"></param>
        /// <returns></returns>
        private static IPAddress ConvertedMask(int maskInt)
        {
            char[] maskChar = new char[32];

            for (int i = 0; i < 32; i++)
            {
                if (i < maskInt)
                {
                    maskChar[i] = '1';
                }
                else
                {
                    maskChar[i] = '0';
                }
            }

            string maskString = new string(maskChar);
            string firstNumb = maskString.Substring(0, 8);
            string secondNumb = maskString.Substring(8, 8);
            string thirdNumb = maskString.Substring(16, 8);
            string fourthNumb = maskString.Substring(24, 8);

            int firstInt = Convert.ToInt32(firstNumb, 2);
            int secondInt = Convert.ToInt32(secondNumb, 2);
            int thirdInt = Convert.ToInt32(thirdNumb, 2);
            int fourthInt = Convert.ToInt32(fourthNumb, 2);

            var numberedMask = $"{firstInt}.{secondInt}.{thirdInt}.{fourthInt}";

            var mask = IPAddress.Parse(numberedMask);

            return mask;
        }
    }
}
