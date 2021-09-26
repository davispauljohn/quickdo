using quickdo_terminal.Extensions;
using quickdo_terminal.Interfaces;
using quickdo_terminal.Models;
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
            string argument = new string[] { "+", "!", "x", "-", ">" }.Contains(command) && args.Length > 1 ? args[1] : null;
            List<string> options = command == "?" ? args.Skip(1).Where(a => a.StartsWith("-")).ToList() : new List<string>();

            switch (command)
            {
                case "?":
                    var isReversed = options.Contains("-r") || options.Contains("--reverse");
                    output = documentService.Query(isReversed: isReversed).ToConsole();
                    break;

                case "!":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.FocusTask(rank);
                        else
                            output.Add(new ConsoleLine("Focus command requires an integer argument `! {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.Query(rank: 1)?.SingleOrDefault();
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case "+":
                    if (!string.IsNullOrEmpty(argument))
                        documentService.AddTask(argument);
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCREATED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case "x":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CompleteTask(rank);
                        else
                            output.Add(new ConsoleLine("Complete command requires an integer argument `x {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCOMPLETED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case "-":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CancelTask(rank);
                        else
                            output.Add(new ConsoleLine("Cancel command requires an integer argument `- {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCANCELLED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case ">":
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.PushTask(rank);
                        else
                            output.Add(new ConsoleLine("Push command requires an integer argument `> {rank:int}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKPUSHED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                default:
                    output.Add(new ConsoleLine("Command not recognised", ConsoleColor.Red));
                    break;
            }

            return output;
        }
    }
}