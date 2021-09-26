using quickdo_terminal.Models;
using quickdo_terminal.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal.Extensions
{
    public static class TaskModelExtensions
    {
        public static ConsoleLine ToConsoleLine(this TaskModel task)
        {
            return new ConsoleLine($"{task.Rank}\t{task.Status}\t{task.Description}", task.Colour);
        }

        public static List<ConsoleLine> ToConsole(this List<TaskModel> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return new List<ConsoleLine>();

            return tasks.Select(t => t.ToConsoleLine()).ToList();
        }

    }
}