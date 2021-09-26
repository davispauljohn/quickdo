using NodaTime;
using quickdo_terminal.Types;
using System;

namespace quickdo_terminal.Models
{
    public class LogModel
    {
        public ZonedDateTime Timestamp { get; set; }
        public string Type { get; set; }
        public Guid? TaskId { get; set; }
        public string Value { get; set; }

        private LogModel()
        {
        }

        public static LogModel FromLogEntry(LogEntry log)
        {
            return new LogModel
            {
                Timestamp = log.Timestamp,
                Type = Enum.GetName(log.Type),
                TaskId = log.TaskId,
                Value = log.Value
            };
        }
    }
}