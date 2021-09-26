using quickdo_terminal.Interfaces;
using quickdo_terminal.Models;
using quickdo_terminal.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository repository;

        public DocumentService(IDocumentRepository repository)
        {
            this.repository = repository;
        }


        public void AddTask(string description)
        {
            Document document = repository.GetDocument();
            document.AddTask(description);
            repository.UpdateDocument(document);
        }

        public void FocusTask(int rank)
        {
            Document document = repository.GetDocument();
            document.FocusTask(rank);
            repository.UpdateDocument(document);
        }

        public void CompleteTask(int rank)
        {
            Document document = repository.GetDocument();
            document.CompleteTask(rank);
            repository.UpdateDocument(document);
        }

        public void CancelTask(int rank)
        {
            Document document = repository.GetDocument();
            document.CancelTask(rank);
            repository.UpdateDocument(document);
        }

        public List<TaskModel> Query(int? rank = null, bool? isReversed = false)
        {
            Document document = repository.GetDocument();
            List<Task> tasks = document.Tasks.OrderBy(t => t.Rank).ToList();

            if (rank.HasValue)
                tasks = tasks.Where(t => t.Rank == rank.Value).ToList();

            if (tasks == null || tasks.Count == 0)
                return new List<TaskModel> { TaskModel.Default() };

            if (isReversed ?? false)
                tasks.Reverse();

            return tasks.Select(task => TaskModel.FromTask(task)).ToList();
        }

        public TaskModel QueryMostRecent(QuickDoLogType? logType)
        {
            Document document = repository.GetDocument();
            IEnumerable<Task> tasks = document.Tasks;
            if (tasks == null)
                return TaskModel.Default();

            var logs = tasks.SelectMany(t => t.Log);
            var taskId = logs.Aggregate((max, curr) =>
                max.Timestamp.ToInstant() < curr.Timestamp.ToInstant()
                && curr.Type == logType ? curr : max).TaskId;
            var task = tasks?.SingleOrDefault(t => t.Id == taskId);

            if (task != null)
                return TaskModel.FromTask(task);

            return TaskModel.Default();
        }

        public void PushTask(int rank)
        {
            Document document = repository.GetDocument();
            document.Push(rank);
            repository.UpdateDocument(document);
        }
    }
}