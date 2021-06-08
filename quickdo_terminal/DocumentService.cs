using quickdo_terminal.Interfaces;
using quickdo_terminal.Models;
using quickdo_terminal.Types;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository repository = new JsonRepository();

        public List<TaskModel> Query(int? top = null, int? rank = null, QuickDoStatus? status = null, bool isDescending = false)
        {
            Document document = repository.GetCurrentDocument();
            IEnumerable<Task> tasks = document.Tasks;

            tasks = isDescending ? tasks.OrderByDescending(t => t.Rank) : tasks.OrderBy(t => t.Rank);
            tasks = rank == null ? tasks : tasks.Where(t => t.Rank == rank.Value).ToList();
            tasks = status == null ? tasks : tasks.Where(t => t.Status == status.Value).ToList();
            tasks = top == null ? tasks : tasks.Take(top.Value).ToList();

            return tasks.Select(task => TaskModel.FromTask(task)).ToList();
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
    }
}