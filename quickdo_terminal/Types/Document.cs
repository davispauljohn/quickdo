using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace quickdo_terminal.Types
{
    public class Document
    {
        public RankedCollection<Task> Tasks { get; set; } = new();
        public List<LogEntry> Log { get; set; } = new();
        public LocalDate Datestamp { get; set; } = LocalDate.FromDateTime(DateTime.Now);

        [JsonConstructor]
        public Document() { }

        public static Document Create()
        {
            var document = new Document();
            document.AddLog(LogEntry.DocumentCreated());

            return document;
        }

        public void AddLog(LogEntry logEntry)
        {
            Log.Add(logEntry);
        }

        public void AddTask(string description)
        {
            var lowestTodoTask = Tasks.OrderByDescending(task => task.Rank).FirstOrDefault(task => task.Status == QuickDoStatus.TODO);
            var targetRank = lowestTodoTask != null ? lowestTodoTask.Rank + 1 : 1;
            var task = Task.Create(description);

            Tasks.Add(task, targetRank);

            Log.Add(LogEntry.TaskCreated(task));
        }

        public void MigrateTask(Task task)
        {
            var lowestTodoTask = Tasks.OrderByDescending(task => task.Rank).FirstOrDefault(task => task.Status == QuickDoStatus.TODO);
            var targetRank = lowestTodoTask != null ? lowestTodoTask.Rank + 1 : 1;

            task.Status = QuickDoStatus.TODO;
            Tasks.Add(task,targetRank);

            Log.Add(LogEntry.TaskPushed(task));
        }

        internal void CompleteTask(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);

            task.Status = QuickDoStatus.DONE;

            Log.Add(LogEntry.TaskCompleted(task));
        }

        internal void CancelTask(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);
            task.Status = QuickDoStatus.NOPE;

            Log.Add(LogEntry.TaskCancelled(task));
        }

        public void FocusTask(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);

            Log.Add(LogEntry.TaskFocused(task));
        }

        public void Push(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);
            task.Status = QuickDoStatus.PUSH;

            Log.Add(LogEntry.TaskPushed(task));
        }

        
    }
}