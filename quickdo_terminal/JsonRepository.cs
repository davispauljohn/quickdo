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
        private const int daysInWeek = 7;
        private JsonSerializerOptions options = new();
        private readonly string directory;

        public JsonRepository()
        {
            options.Converters.Add(new JsonStringEnumConverter());
            options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.WriteIndented = true;

            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qdo", "documents");
            var documentPath = Path.Combine(directory, string.Concat(DateTime.Now.Date.ToString("yyyyMMdd"), ".json"));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(documentPath))
            {
                var currentDocument = Document.Create();
                var previousDocument = GetPreviousDocument();
                if (previousDocument != null)
                    previousDocument.Tasks.Where(t => t.Status == QuickDoStatus.PUSH).ToList().ForEach(t => currentDocument.MigrateTask(t));
                File.WriteAllText(documentPath, JsonSerializer.Serialize(currentDocument, options));
            }
        }

        public Document GetDocument(int daysAgo = 0)
        {
            string documentPath = Path.Combine(directory, string.Concat(DateTime.Now.AddDays(-daysAgo).Date.ToString("yyyyMMdd"), ".json")); ;
            
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

        public Document GetPreviousDocument()
        {
            string documentPath = null;
            int daysAgo = 1;
            while (documentPath == null)
            {
                documentPath = Path.Combine(directory, string.Concat(DateTime.Now.AddDays(-daysAgo).Date.ToString("yyyyMMdd"), ".json"));
                if (!File.Exists(documentPath))
                {
                    documentPath = null;
                    daysAgo++;
                }

                if (daysAgo == 8)
                    return null;
            }

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
            var documentPath = Path.Combine(directory, string.Concat(DateTime.Now.Date.ToString("yyyyMMdd"), ".json"));
            File.WriteAllText(documentPath, JsonSerializer.Serialize(document, options));
        }
    }
}