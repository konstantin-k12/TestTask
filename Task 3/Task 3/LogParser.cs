using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public static class LogParser
{
    // Метод для парсинга строк формата 1 и формата 2
    public static List<LogEntry> ParseLogFile(string inputFilePath, out List<string> invalidLines)
    {
        var logs = new List<LogEntry>();

        // Для хранения неверных записей
        invalidLines = new List<string>(); 
        string[] lines = File.ReadAllLines(inputFilePath);

        foreach (var line in lines)
        {
            try
            {
                LogEntry logEntry = null;
                // Формат 1
                if (Regex.IsMatch(line, @"^\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d{3} (INFO|INFORMATION|ERROR|WARN|DEBUG)"))
                {
                    logEntry = ParseFormatOne(line);
                }
                // Формат 2
                else if (Regex.IsMatch(line, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4}\| (INFO|WARN|ERROR|DEBUG)\|\d+\|"))
                {
                    logEntry = ParseFormatTwo(line);
                }

                // Если запись распарсилась, добавляем её в список
                if (logEntry != null)
                {
                    logs.Add(logEntry);
                }
                else
                {
                    // Запись невалидна
                    invalidLines.Add(line); 
                }
            }
            catch (Exception)
            {
                // В случае исключения, добавляем в список ошибок
                invalidLines.Add(line); 
            }
        }

        return logs;
    }

    // Парсинг Формата 1
    private static LogEntry ParseFormatOne(string line)
    {
        var match = Regex.Match(line, @"^(?<date>\d{2}\.\d{2}\.\d{4}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<level>INFO|INFORMATION|ERROR|WARN|DEBUG) (?<message>.+)$");

        if (match.Success)
        {
            var date = DateTime.ParseExact(match.Groups["date"].Value, "dd.MM.yyyy", null);
            var time = match.Groups["time"].Value;
            var level = MapLogLevel(match.Groups["level"].Value);
            var message = match.Groups["message"].Value;

            return new LogEntry(date, time, level, "DEFAULT", message);
        }

        return null;
    }

    // Парсинг Формата 2
    private static LogEntry ParseFormatTwo(string line)
    {
        var match = Regex.Match(line, @"^(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d{4})\| (?<level>INFO|WARN|ERROR|DEBUG)\|\d+\|(?<method>.+?)\| (?<message>.+)$");

        if (match.Success)
        {
            var date = DateTime.ParseExact(match.Groups["date"].Value, "yyyy-MM-dd", null);
            var time = match.Groups["time"].Value;
            var level = MapLogLevel(match.Groups["level"].Value);
            var method = match.Groups["method"].Value;
            var message = match.Groups["message"].Value;

            return new LogEntry(date, time, level, method, message);
        }

        return null;
    }

    // Метод для преобразования уровня логирования
    private static string MapLogLevel(string level)
    {
        return level switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            _ => level,
        };
    }
}
