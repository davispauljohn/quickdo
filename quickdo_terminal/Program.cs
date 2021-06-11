using quickdo_terminal.Extensions;
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
            IDocumentService service = new DocumentService();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string command = args[0];
            char primary = command[0];
            string argument = command.Substring(1);
            string[] options = args.Skip(1).ToArray();

            if(command.ToLowerInvariant() == "-h" || command.ToLowerInvariant() == "--help")
            {
                Console.WriteLine("TODO: Impluhment halp");
            }

            switch (primary)
            {
                case '?':
                    service.Query().ToConsole();
                    break;

                case '!':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.FocusTask(rank);
                        else
                            Console.Error.WriteLine("Error: Only integer arguments are accepted when focusing a task !{rank:int}");
                    else
                        service.Query(rank: 1, status: QuickDoStatus.TODO).ToConsole();
                    break;

                case '+':
                    if (!string.IsNullOrEmpty(argument) && !string.IsNullOrEmpty(argument))
                        service.AddTask(argument);
                    else
                        service.Query(top: 1, status: QuickDoStatus.TODO, isDescending: true).ToConsole();
                    break;

                case 'x':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CompleteTask(rank);
                        else
                            Console.Error.WriteLine("Error: Only integer arguments are accepted when completing a task (x{rank:int})");
                    else
                        service.Query(top: 1, status: QuickDoStatus.DONE, isDescending: true).ToConsole();
                    break;

                case '-':
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            service.CancelTask(rank);
                        else
                            Console.Error.WriteLine("Error: Only integer arguments are accepted when cancelling a task (-{rank:int})");
                    else
                        service.Query(top: 1, status: QuickDoStatus.NOPE, isDescending: true).ToConsole();
                    break;
                default:
                    Console.Error.WriteLine("Error: Command not recognised. qdo --help");
                    break;

            }
        }
    }
}