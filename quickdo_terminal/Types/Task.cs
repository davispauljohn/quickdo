using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace quickdo_terminal.Types
{
    public class Task
    {
        public Guid Id { get; set; }
        public QuickDoStatus Status { get; set; }
        public int Rank { get; set; }
        public string Description { get; set; }
        public List<LogEntry> Log { get; set; } = new();

        [JsonConstructor]
        public Task() { }

        public static Task Create(string description, int rank)
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Status = QuickDoStatus.TODO,
                Rank = rank,
                Description = description
            };
        }
    }
}