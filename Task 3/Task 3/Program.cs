using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Путь к входным и выходным файлам
        string inputFilePath = "input.log";
        string outputFilePath = "output.log";
        string problemFilePath = "problems.txt";

        // Парсим входной файл
        var invalidLines = new List<string>();
        var logs = LogParser.ParseLogFile(inputFilePath, out invalidLines);

        // Запись результатов в выходной файл
        using (var writer = new StreamWriter(outputFilePath))
        {
            foreach (var log in logs)
            {
                writer.WriteLine($"{log.Date:dd-MM-yyyy}\t{log.Time}\t{log.Level}\t{log.Method}\t{log.Message}");
            }
        }

        // Запись неверных записей в файл ошибок
        if (invalidLines.Count > 0)
        {
            using (var writer = new StreamWriter(problemFilePath))
            {
                foreach (var line in invalidLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        Console.WriteLine("Обработка завершена!");
    }
}