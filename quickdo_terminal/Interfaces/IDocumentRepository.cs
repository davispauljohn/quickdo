using quickdo_terminal.Types;

namespace quickdo_terminal.Interfaces
{
    internal interface IDocumentRepository
    {
        Document GetCurrentDocument();

        void UpdateDocument(Document document);
    }
}