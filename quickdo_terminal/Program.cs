using System;
using System.Linq;

namespace quickdo_terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string command = args[0];
            char primary = command[0];
            string argument = command.Substring(1);
            string[] options = args.Skip(1).ToArray();
            DocumentService service = new DocumentService();

            switch (primary)
            {
                case '?':
                    var tasks = service.Query();
                    foreach (var task in tasks)
                    {
                        Console.ForegroundColor = task.Colour;
                        Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                        
                    break;

                case '!':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.FocusTask(rank);
                        else
                            throw new ArgumentException("Only int arguments are accepted when focusing a task  (![rank:int])");
                    else
                        Console.WriteLine("Focussed Task (Query)");
                    break;

                case '+':
                    if (!string.IsNullOrEmpty(argument) && !string.IsNullOrEmpty(argument))
                        service.AddTask(argument);
                    else
                        Console.WriteLine($"Last Added Task (Query)");
                    break;

                case 'x':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CompleteTask(rank);
                        else
                            throw new ArgumentException("Only int arguments are accepted when completing a task  (x[rank:int])");
                    else
                        Console.WriteLine("Last Completed Task (Query)");
                    break;

                case '-':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CancelTask(rank);
                        else
                            throw new ArgumentException("Only int arguments are accepted when cancelling a task (-[rank:int])");
                    else
                        Console.WriteLine("Cancel Task (Query)");
                    break;
            }
        }
    }
}