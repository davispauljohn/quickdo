using quickdo_terminal.Interfaces;
using quickdo_terminal.Types;
using System;
using System.Linq;

namespace quickdo_terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string command = args[0];
            char primary = command[0];
            string argument = command.Substring(1);
            string[] options = args.Skip(1).ToArray();
            IDocumentService service = new DocumentService();

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
                    {
                        var focussedTask = service.Query(rank: 1, status: QuickDoStatus.TODO);
                        foreach (var task in focussedTask)
                        {
                            Console.ForegroundColor = task.Colour;
                            Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    break;

                case '+':
                    if (!string.IsNullOrEmpty(argument) && !string.IsNullOrEmpty(argument))
                        service.AddTask(argument);
                    else
                    {
                        var focussedTask = service.Query(top: 1, status: QuickDoStatus.TODO, isDescending: true);
                        foreach (var task in focussedTask)
                        {
                            Console.ForegroundColor = task.Colour;
                            Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    break;

                case 'x':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CompleteTask(rank);
                        else
                            throw new ArgumentException("Only int arguments are accepted when completing a task  (x[rank:int])");
                    else
                    {
                        var focussedTask = service.Query(top: 1, status: QuickDoStatus.DONE, isDescending: true);
                        foreach (var task in focussedTask)
                        {
                            Console.ForegroundColor = task.Colour;
                            Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    break;

                case '-':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CancelTask(rank);
                        else
                            throw new ArgumentException("Only int arguments are accepted when cancelling a task (-[rank:int])");
                    else
                    {
                        var focussedTask = service.Query(top: 1, status: QuickDoStatus.NOPE, isDescending: true);
                        foreach (var task in focussedTask)
                        {
                            Console.ForegroundColor = task.Colour;
                            Console.WriteLine($"{task.Rank}\t{task.Status}\t{task.Description}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    break;
            }
        }
    }
}