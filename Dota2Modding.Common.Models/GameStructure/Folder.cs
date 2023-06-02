using SteamDatabase.ValvePak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Folder : FolderView
    {
        public Folder(string name, Folder parent) : this(name, parent, false)
        {
        }
        public Folder(string name, Folder parent, bool isVirtual)
        {
            Name = name;
            Parent = parent;
            IsVirtual = isVirtual;
        }

        public bool IsVirtual { get; }

        public Folder Parent { get; }

        public string Name { get; }

        public Dictionary<string, Folder> Folders { get; set; } = new();

        public List<Entry> Entries { get; set; } = new();

        public FolderItemType ItemType => FolderItemType.Folder;

        public string DisplayName => Name;

        public FolderView this[string item]
        {
            get => Folders[item];
        }

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
            var splitter = entry.Source.IsVpk ? Package.DirectorySeparatorChar : Path.DirectorySeparatorChar;
            var spliiterPos = path.IndexOf(splitter);
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
                    Folders.Add(aim, new(aim, this, entry.Source.IsVpk) { { resume, entry } });
                }
            }
            else
            {
                Entries.Add(entry);
            }
        }

        public string Sanitize(string path)
        {
            var vpkSlash = path.IndexOf(Package.DirectorySeparatorChar);
            var osSlash = path.IndexOf(Path.DirectorySeparatorChar);
            if (vpkSlash != osSlash)
            {
                var slashPos = vpkSlash == -1
                    ? osSlash
                    : osSlash == -1
                    ? vpkSlash
                    : vpkSlash < osSlash ? vpkSlash : osSlash;
                var baseFolder = path[..slashPos];
                if (Folders.TryGetValue(baseFolder, out var folder))
                {
                    if (folder.IsVirtual)
                    {
                        return path.Replace(Path.DirectorySeparatorChar, Package.DirectorySeparatorChar);
                    }
                    else
                    {
                        return path.Replace(Package.DirectorySeparatorChar, Path.DirectorySeparatorChar);
                    }
                }
            }
            return path;
        }

        public IEnumerator<FolderView> GetEnumerator() => EnumerateFolderView().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
