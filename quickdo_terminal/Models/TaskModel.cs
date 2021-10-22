using quickdo_terminal.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public int Rank { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ConsoleColor Colour { get; internal set; }
        public List<LogModel> Log { get; private set; }

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
                Colour = task.Status == QuickDoStatus.NOPE
                    ? ConsoleColor.Red
                    : task.Status == QuickDoStatus.DONE
                        ? ConsoleColor.Green
                        : task.Status == QuickDoStatus.PUSH
                            ? ConsoleColor.Blue
                            : ConsoleColor.White,
                Log = task.Log.Select(l => LogModel.FromLogEntry(l)).ToList()
            };
        }

        public static TaskModel Default()
        {
            return new TaskModel();
        }
    }
}