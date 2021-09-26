using quickdo_terminal.Extensions;
using quickdo_terminal.Interfaces;
using quickdo_terminal.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal
{
    public class InputService : IInputService
    {
        private readonly IDocumentService documentService;

        public InputService(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        public List<ConsoleLine> ParseAndRunInput(string[] args)
        {
            List<ConsoleLine> output = new();

            if (args.Length == 0 || args.Contains("-h") || args.Contains("--help"))
            {
                output.Add(new ConsoleLine("TODO: Impluhment halp", ConsoleColor.White));
                return output;
            }

            string command = args[0];
            string argument = args.Length > 1 ? args[1] : null;


            switch (command)
            {
                case "?":
                    output = documentService.Query().ToConsole();
                    break;

                case "!":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.FocusTask(rank);
                        else
                            output.Add(new ConsoleLine("Error: Only integer arguments are accepted when focusing a task `! {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.Query(rank: 1)?.SingleOrDefault();
                        if (task != null)
                            output.Add(task.ToConsole());
                    }
                    break;

                case "+":
                    if (!string.IsNullOrEmpty(argument))
                        documentService.AddTask(argument);
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoStatus.TODO);
                        if (task != null)
                            output.Add(task.ToConsole());
                    }
                    break;

                case "x":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CompleteTask(rank);
                        else
                            output.Add(new ConsoleLine("Error: Only integer arguments are accepted when completing a task `x {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoStatus.DONE);
                        if (task != null)
                            output.Add(task.ToConsole());
                    }
                    break;

                case "-":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CancelTask(rank);
                        else
                            output.Add(new ConsoleLine("Error: Only integer arguments are accepted when cancelling a task `- {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoStatus.NOPE);
                        if (task != null)
                            output.Add(task.ToConsole());
                    }
                    break;

                default:
                    output.Add(new ConsoleLine("Error: Command not recognised. qdo --help", ConsoleColor.Red));
                    break;
            }
            return output;
        }
    }
}