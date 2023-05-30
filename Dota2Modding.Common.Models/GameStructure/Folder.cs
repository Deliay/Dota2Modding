﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Folder : FolderView
    {
        public Folder(string name, Folder parent)
        {
            Name = name;
            Parent = parent;
        }

        public Folder Parent { get; }

        public string Name { get; }

        public Dictionary<string, Folder> Folders { get; set; } = new();

        public List<Entry> Entries { get; set; } = new();

        public FolderItemType ItemType => FolderItemType.Folder;

        public string DisplayName => Name;

        private IEnumerable<FolderView> EnumerateFolderView()
        {
            foreach (var folder in Folders.Values)
            {
                yield return folder;
            }

            foreach (var file in Entries)
            {
                yield return file;
            }
        }

        public void Add(string path, Entry entry)
        {
            var spliiterPos = path.IndexOf('/');
            if (spliiterPos != -1)
            {
                var aim = path[0..spliiterPos];
                var resume = path[(spliiterPos + 1)..];

                if (Folders.TryGetValue(aim, out var aimFolder))
                {
                    aimFolder.Add(resume, entry);
                }
                else if (aim == " " || aim == "")
                {
                    Entries.Add(entry);
                }
                else
                {
                    Folders.Add(aim, new(aim, this) { { resume, entry } });
                }
            }
            else
            {
                Entries.Add(entry);
            }
        }

        public IEnumerator<FolderView> GetEnumerator() => EnumerateFolderView().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}