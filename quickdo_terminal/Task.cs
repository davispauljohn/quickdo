using System;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    public class Task
    {
        public Guid Id { get; set; }
        public QuickDoStatus Status { get; set; }
        public int Rank { get; set; }
        public string Description { get; set; }

        [JsonConstructor]
        public Task() { }

        public static Task Create(string description, int rank)
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Status = QuickDoStatus.DO,
                Rank = rank,
                Description = description
            };
        }
    }
}