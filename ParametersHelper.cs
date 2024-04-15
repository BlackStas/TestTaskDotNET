namespace TestTaskDotNET
{
    internal static class ParametersHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="argumentName"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        internal static string? GetValueFromArgsByArgName(string[] args, string argumentName, bool isRequired)
        {
            var arg = args.FirstOrDefault(x => x.Contains(argumentName));

            if (arg == null && isRequired) 
            {
                throw new Exception($"Обязательынй аргумент {argumentName} не указан");
            }

            if (arg == null && !isRequired)
            {
                return null;
            }

            string? value = null;
            if (arg.Split("=").Length != 1)
            {
                value = arg.Split("=")[1];
            }
            return value;
        }

        /// <summary>
        /// Проверка наличия даты и ее корректность.
        /// </summary>
        internal static DateTime GetDate(string dateArgument)
        {
            if (!DateTime.TryParse(dateArgument, out DateTime date))
            {
                throw new Exception($"Неверно указан параметр даты {dateArgument}.");
            }

            return date;
        }
    }
}
