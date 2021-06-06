using System.Threading.Tasks;

namespace quickdo_terminal
{
    internal interface IDocumentRepository
    {
        Document GetCurrentDocument();

        void UpdateDocument(Document document);
    }
}