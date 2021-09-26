using NodaTime;
using System;
using System.Text.Json.Serialization;

namespace quickdo_terminal.Types
{
    public class LogEntry
    {
        public ZonedDateTime Timestamp { get; set; }
        public QuickDoLogType Type { get; set; }
        public Guid? TaskId { get; set; }
        public string Value { get; set; }

        [JsonConstructor]
        public LogEntry() { }

        public static LogEntry QueryExecuted(string query)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.QUERYEXECUTED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = default,
                Value = query
            };
        }

        public static LogEntry DocumentCreated()
        {
            return new LogEntry
            {
                Type = QuickDoLogType.DOCUMENTCREATED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = default,
                Value = string.Empty
            };
        }

        public static LogEntry TaskCreated(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TASKCREATED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = string.Empty
            };
        }

        public static LogEntry TaskCancelled(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TASKCANCELLED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = task.Description
            };
        }

        internal static LogEntry TaskPushed(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TASKPUSHED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = string.Empty
            };
        }

        public static LogEntry TaskCompleted(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TASKCOMPLETED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = Enum.GetName(task.Status)
            };
        }

        public static LogEntry TaskFocused(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.TASKFOCUSED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = task.Rank.ToString()
            };
        }
        public static LogEntry RankChanged(Task task)
        {
            return new LogEntry
            {
                Type = QuickDoLogType.RANKCHANGED,
                Timestamp = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now),
                TaskId = task.Id,
                Value = task.Rank.ToString()
            };
        }
    }
}