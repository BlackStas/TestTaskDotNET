namespace TestTaskDotNET
{
    /// <summary>
    /// Вспомогательный класс для работы с параметрами.
    /// </summary>
    internal static class ParametersHelper
    {
        /// <summary>
        /// Получение параметра из параметров запуска.
        /// </summary>
        /// <param name="args">Параметры запуска.</param>
        /// <param name="argumentName">Имя параметра.</param>
        /// <param name="isRequired">Обязательный ли параметр.</param>
        /// <returns>Значение параметра.</returns>
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
        /// <param name="dateArgument">Строковое представление даты.</param>
        /// <returns>Дату и время.</returns>
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
