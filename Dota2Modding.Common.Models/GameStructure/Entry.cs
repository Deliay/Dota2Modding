using Dota2Modding.Common.Models.Searching;
using SteamDatabase.ValvePak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Entry : IFullContextSearchable, FolderView
    {
        public Source Source { get; set; }
        public string FullName { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }

        public FolderItemType ItemType => FolderItemType.File;

        public string DisplayName => $"{Name}.{Extension}";

        public FolderView this[string item] => throw new InvalidOperationException();

        public string GetContainsText()
        {
            return $"{FullName} {Name}.{Extension} {Extension} {Source.Path} {Path}";
        }

        public string GetFullPath() => System.IO.Path.Combine(Source.Path, FullName);

        public string GetPath() => Source.IsVpk
            ? $"{Path}{Package.DirectorySeparatorChar}{DisplayName}"
            : System.IO.Path.Combine(Path, DisplayName);

        public IEnumerator<FolderView> GetEnumerator() => Enumerable.Empty<FolderView>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
