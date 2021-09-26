using quickdo_terminal.Models;
using quickdo_terminal.Types;
using System.Collections.Generic;

namespace quickdo_terminal.Interfaces
{
    public interface IDocumentService
    {
        void AddTask(string description);
        void CancelTask(int rank);
        void CompleteTask(int rank);
        void FocusTask(int rank);
        List<TaskModel> Query(int? rank = null, bool? isReversed = false);
        TaskModel QueryMostRecent(QuickDoLogType? logType);
    }
}