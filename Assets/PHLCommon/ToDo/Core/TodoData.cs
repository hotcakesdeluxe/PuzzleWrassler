using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PHL.Common.Todo
{
    [Serializable]
    public class TodoData
    {
        public bool autoScan;
        public List<TodoEntry> entries = new List<TodoEntry>();

        public List<string> tagsList = new List<string>()
        {
            "TODO"
        };

        public int entriesCount
        {
            get { return entries.Count; }
        }

        public int tagsCount
        {
            get { return tagsList.Count; }
        }

        public int GetCountByTag(int tag)
        {
            return tag != -1 ? entries.Count(e => e.Tag == tagsList[tag]) : entriesCount;
        }

        public TodoEntry GetEntryAt(int index)
        {
            return entries[index];
        }

        public void AddTag(string tag)
        {
            if (tagsList.Contains(tag) || string.IsNullOrEmpty(tag))
            {
                return;
            }

            tagsList.Add(tag);
        }

        public void RemoveTag(int index)
        {
            if (tagsList.Count >= (index + 1))
            {
                tagsList.RemoveAt(index);
            }
        }
    }

}