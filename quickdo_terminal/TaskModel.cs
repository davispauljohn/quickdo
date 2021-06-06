using System;

namespace quickdo_terminal
{
    internal class TaskModel
    {
        public int Rank { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ConsoleColor Colour { get; internal set; }

        private TaskModel()
        {
        }

        public static TaskModel FromTask(Task task)
        {
            return new TaskModel
            {
                Rank = task.Rank,
                Status = Enum.GetName(task.Status),
                Description = task.Description,
                Colour = task.Status == QuickDoStatus.NOPE ? ConsoleColor.Red : task.Status == QuickDoStatus.DONE ? ConsoleColor.Green : ConsoleColor.White
            };
        }
    }
}