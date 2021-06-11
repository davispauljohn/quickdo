﻿using NodaTime;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace quickdo_terminal.Types
{
    public class Document
    {
        public List<Task> Tasks { get; set; } = new();
        public List<LogEntry> Log { get; set; } = new();
        public LocalDate Datestamp { get; private set; } = LocalDate.FromDateTime(DateTime.Now);

        [JsonConstructor]
        public Document() { }

        public static Document Create()
        {
            var document = new Document();
            document.AddLog(LogEntry.DocumentCreated());

            return document;
        }

        public void AddLog(LogEntry logEntry)
        {
            Log.Add(logEntry);
        }

        public void AddTask(string description)
        {
            var tempRank = Tasks.Count + 1;
            var incompleteTasks = Tasks.Where(task => task.Status == QuickDoStatus.TODO);
            var targetRank = incompleteTasks.Any() ? incompleteTasks.Max(task => task.Rank) + 1 : 1;
            var task = Task.Create(description, tempRank);

            Tasks.Add(task);
            MarshalTasks(tempRank, targetRank);

            Log.Add(LogEntry.TaskCreated(task));
            Log.Add(LogEntry.DescriptionChanged(task));
            Log.Add(LogEntry.StatusChanged(task));
            Log.Add(LogEntry.RankChanged(task));
        }

        internal void CompleteTask(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);

            task.Status = QuickDoStatus.DONE;
            MarshalTasks(rank);

            Log.Add(LogEntry.StatusChanged(task));
        }

        internal void CancelTask(int rank)
        {
            var task = Tasks.SingleOrDefault(task => task.Rank == rank);
            task.Status = QuickDoStatus.NOPE;
            MarshalTasks(rank);

            Log.Add(LogEntry.StatusChanged(task));
        }

        public void FocusTask(int rank)
        {
            MarshalTasks(rank, 1);
        }

        private void MarshalTasks(int oldRank, int? newRank = null)
        {
            int rank = newRank ?? Tasks.Count;

            if (oldRank == rank) return;

            bool isPromotion = oldRank > rank;
            Task target = Tasks.Single(task => task.Rank == oldRank);

            foreach (var task in Tasks)
            {
                if (isPromotion && task.Rank < oldRank && task.Rank >= rank)
                {
                    task.Rank += 1;
                    Log.Add(LogEntry.RankChanged(task));
                }

                if (!isPromotion && task.Rank > oldRank && task.Rank <= rank)
                {
                    task.Rank -= 1;
                    Log.Add(LogEntry.RankChanged(task));
                }

                if (task.Id == target.Id)
                {
                    task.Rank = rank;
                    Log.Add(LogEntry.RankChanged(task));
                }
            }
        }
    }
}