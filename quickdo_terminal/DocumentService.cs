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
            Document document = repository.GetCurrentDocument();
            document.AddTask(description);
            repository.UpdateDocument(document);
        }

        public void FocusTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.FocusTask(rank);
            repository.UpdateDocument(document);
        }

        public void CompleteTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.CompleteTask(rank);
            repository.UpdateDocument(document);
        }

        public void CancelTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.CancelTask(rank);
            repository.UpdateDocument(document);
        }

        public List<TaskModel> Query(int? rank = null)
        {
            Document document = repository.GetCurrentDocument();
            IEnumerable<Task> tasks = document.Tasks;

            tasks = rank == null ? tasks : tasks.Where(t => t.Rank == rank.Value).ToList();

            return tasks.Select(task => TaskModel.FromTask(task)).ToList();
        }

        public TaskModel QueryMostRecent(QuickDoStatus? status = null)
        {
            Document document = repository.GetCurrentDocument();
            IEnumerable<Task> tasks = document.Tasks;
            if (tasks == null)
                return null;

            var logs = tasks.SelectMany(t => t.Log);
            var taskId = logs.Aggregate((max, curr) =>
                max.Timestamp.ToInstant() < curr.Timestamp.ToInstant()
                && tasks.SingleOrDefault(t => t.Id == curr.TaskId)?.Status == status
                && curr.Type == QuickDoLogType.STATUSCHANGED ? curr : max).TaskId;
            var task = tasks?.SingleOrDefault(t => t.Id == taskId);

            if (task != null)
                return TaskModel.FromTask(task);

            return null;
        }
    }
}