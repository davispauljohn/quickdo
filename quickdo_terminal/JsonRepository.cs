using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using quickdo_terminal.Interfaces;
using quickdo_terminal.Types;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    internal class JsonRepository : IDocumentRepository
    {
        private JsonSerializerOptions options = new();
        private readonly string directory;
        private string documentName = string.Concat(DateTime.Now.Date.ToString("yyyyMMdd"), ".json");
        private readonly string documentPath;

        public JsonRepository()
        {
            options.Converters.Add(new JsonStringEnumConverter());
            options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.WriteIndented = true;

            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qdo", "documents");
            documentPath = Path.Combine(directory, documentName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(documentPath))
            {
                var currentDocument = Document.Create();
                var previousDocument = GetDocument(1);
                if (previousDocument != null)
                    previousDocument.Tasks.Where(t => t.Status == QuickDoStatus.PUSH).ToList().ForEach(t => currentDocument.MigrateTask(t));
                File.WriteAllText(documentPath, JsonSerializer.Serialize(currentDocument, options));
            }
        }

        public Document GetDocument(int daysAgo = 0)
        {
            if(daysAgo > 0)
                documentName = string.Concat(DateTime.Now.AddDays(1).Date.ToString("yyyyMMdd"), ".json");

            if (!File.Exists(documentPath))
                return null;

            var data = File.ReadAllText(documentPath);
            var document = JsonSerializer.Deserialize<Document>(data, options);

            foreach (var task in document.Tasks)
            {
                task.Log = document.Log.Where(log => log.TaskId == task.Id).ToList();
            }

            return document;
        }

        public void UpdateDocument(Document document)
        {
            File.WriteAllText(documentPath, JsonSerializer.Serialize(document, options));
        }
    }
}