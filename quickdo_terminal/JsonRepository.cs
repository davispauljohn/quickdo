﻿using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    internal class JsonRepository : IDocumentRepository
    {
        private JsonSerializerOptions options = new();
        private readonly string directory;
        private readonly string currentDocumentName = string.Concat(DateTime.Now.Date.ToString("yyyyMMdd"), ".json");
        private readonly string currentDocumentPath;

        public JsonRepository()
        {
            options.Converters.Add(new JsonStringEnumConverter());
            options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb); 
            options.ReferenceHandler = ReferenceHandler.Preserve;
            options.PropertyNamingPolicy = null;

            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qdo", "documents");
            currentDocumentPath = Path.Combine(directory, currentDocumentName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(currentDocumentPath))
                File.WriteAllText(currentDocumentPath, JsonSerializer.Serialize(Document.Create(), options));
        }

        public Document GetCurrentDocument()
        {
            var content = File.ReadAllText(currentDocumentPath);
            return JsonSerializer.Deserialize<Document>(content, options);
        }

        public void UpdateDocument(Document document)
        {
            File.WriteAllText(currentDocumentPath, JsonSerializer.Serialize(document, options));
        }
    }
}