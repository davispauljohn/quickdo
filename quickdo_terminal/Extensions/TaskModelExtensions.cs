using quickdo_terminal.Models;
using System;
using System.Collections.Generic;

namespace quickdo_terminal.Extensions
{
    public static class TaskModelExtensions
    {
        public static void ToConsole(this List<TaskModel> tasks)
        {
            foreach (var task in tasks)
            {
                Console.ForegroundColor = task.Colour;
                Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}