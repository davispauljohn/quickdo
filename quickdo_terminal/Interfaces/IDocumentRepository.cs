using quickdo_terminal.Types;

namespace quickdo_terminal.Interfaces
{
    public interface IDocumentRepository
    {
        Document GetCurrentDocument();

        void UpdateDocument(Document document);
    }
}