using System;

public class LogEntry
{
    public DateTime Date { get; }
    public string Time { get; }
    public string Level { get; }
    public string Method { get; }
    public string Message { get; }

    public LogEntry(DateTime date, string time, string level, string method, string message)
    {
        Date = date;
        Time = time;
        Level = level;
        Method = method ?? "DEFAULT"; // если нет значения у метода, но будет по умолчанию
        Message = message;
    }
}
