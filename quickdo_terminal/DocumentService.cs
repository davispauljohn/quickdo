using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal
{
    internal class DocumentService
    {
        private readonly IDocumentRepository repository = new JsonRepository();

        internal void AddTask(string description)
        {
            Document document = repository.GetCurrentDocument();
            document.AddTask(description);
            repository.UpdateDocument(document);
        }

        internal void FocusTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.FocusTask(rank);
            repository.UpdateDocument(document);
        }

        internal List<TaskModel> Query()
        {
            Document document = repository.GetCurrentDocument();
            return document.Tasks
                .OrderBy(task => task.Rank)
                .ThenBy(task => task.Status)
                .Select(task => TaskModel.FromTask(task))
                .ToList();
        }

        internal void CompleteTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.CompleteTask(rank);
            repository.UpdateDocument(document);
        }

        internal void CancelTask(int rank)
        {
            Document document = repository.GetCurrentDocument();
            document.CancelTask(rank);
            repository.UpdateDocument(document);
        }
    }
}