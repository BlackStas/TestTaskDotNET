using ConsoleApp1;

namespace TestTaskDotNET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileLog = ParametersHelper.GetValueFromArgsByArgName(args, "--file-log", true);
            var fileOutput = ParametersHelper.GetValueFromArgsByArgName(args, "--file-output", true);
            var addressStart = ParametersHelper.GetValueFromArgsByArgName(args, "--address-start", false);
            var addressMask = ParametersHelper.GetValueFromArgsByArgName(args, "--address-mask", false);
            var timeStart = ParametersHelper.GetValueFromArgsByArgName(args, "--time-start", true);
            var timeEnd = ParametersHelper.GetValueFromArgsByArgName(args, "--time-end", true);

            WorkWithFiles workWithFiles = new WorkWithFiles();

            var textFromFile = workWithFiles.ReadFromFile(fileLog);

            var startIp = IpHelper.GetAddressStart(addressStart);

            var sortedText = workWithFiles.SortText(textFromFile, startIp, IpHelper.GetMaskAddress(addressMask, startIp), ParametersHelper.GetDate(timeStart), ParametersHelper.GetDate(timeEnd));

            workWithFiles.SaveText(sortedText, fileOutput);
        }
    }
}