using quickdo_terminal.Types;

namespace quickdo_terminal.Interfaces
{
    public interface IDocumentRepository
    {
        Document GetDocument(int daysAgo = 0);

        void UpdateDocument(Document document);
    }
}