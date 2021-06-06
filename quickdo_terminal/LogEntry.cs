using NodaTime;
using System;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    public class LogEntry
    {
        public Instant Timestamp { get; set; }
        public QuickDoLogType Type { get; set; }
        public Guid TaskId { get; set; }
        public string Value { get; set; }

        [JsonConstructor]
        public LogEntry() { }

        public static LogEntry QueryExecuted(string query)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.QueryExecuted,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = default,
                Value = query
            };
        }

        public static LogEntry DocumentCreated()
        {
            return new LogEntry
            {
                Type = QuickDoLogType.DocumentCreated,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = default,
                Value = string.Empty
            };
        }

        public static LogEntry TaskCreated(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TaskCreated,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = string.Empty
            };
        }

        public static LogEntry DescriptionChanged(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.DescriptionChanged,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = task.Description
            };
        }

        public static LogEntry StatusChanged(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.StatusChanged,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = Enum.GetName(task.Status)
            };
        }

        public static LogEntry RankChanged(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.RankChanged,
                Timestamp = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = task.Rank.ToString()
            };
        }
    }
}