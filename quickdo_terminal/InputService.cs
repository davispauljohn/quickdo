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
        private const string QUERY_TOKEN = "?";
        private const string ADD_TOKEN = "+";
        private const string FOCUS_TOKEN = "!";
        private const string COMPLETE_TOKEN = "-";
        private const string CANCEL_TOKEN = "x";
        private const string PUSH_TOKEN = "~";

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
            string argument = new string[] { ADD_TOKEN, FOCUS_TOKEN, CANCEL_TOKEN, COMPLETE_TOKEN, PUSH_TOKEN }.Contains(command) && args.Length > 1 ? args[1] : null;
            List<string> options = command == QUERY_TOKEN ? args.Skip(1).Where(a => a.StartsWith("-")).ToList() : new List<string>();

            switch (command)
            {
                case QUERY_TOKEN:
                    var isReversed = options.Contains("-r") || options.Contains("--reverse");
                    output = documentService.Query(isReversed: isReversed).ToConsole();
                    break;

                case FOCUS_TOKEN:
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.FocusTask(rank);
                        else
                            output.Add(new ConsoleLine($"Focus command requires an integer argument `{FOCUS_TOKEN} {{rank:int}}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.Query(rank: 1)?.SingleOrDefault();
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case ADD_TOKEN:
                    if (!string.IsNullOrEmpty(argument))
                        documentService.AddTask(argument);
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCREATED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case COMPLETE_TOKEN:
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CompleteTask(rank);
                        else
                            output.Add(new ConsoleLine($"Complete command requires an integer argument `{COMPLETE_TOKEN} {{rank:int}}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCOMPLETED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case CANCEL_TOKEN:
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.CancelTask(rank);
                        else
                            output.Add(new ConsoleLine($"Cancel command requires an integer argument `{CANCEL_TOKEN} {{rank:int}}`", ConsoleColor.Red));
                    else
                    {
                        var task = documentService.QueryMostRecent(QuickDoLogType.TASKCANCELLED);
                        if (task != TaskModel.Default())
                            output.Add(task.ToConsoleLine());
                    }
                    break;

                case PUSH_TOKEN:
                    if (!string.IsNullOrEmpty(argument))
                        if (int.TryParse(argument, out int rank))
                            documentService.PushTask(rank);
                        else
                            output.Add(new ConsoleLine($"Push command requires an integer argument `{PUSH_TOKEN} {{rank:int}}`", ConsoleColor.Red));
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