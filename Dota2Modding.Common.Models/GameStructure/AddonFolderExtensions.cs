using SteamDatabase.ValvePak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public static class AddonFolderExtensions
    {
        private static void AddFolder(this Packages packages, string root, string @base, string path)
        {
            var files = Directory.EnumerateFiles(path);

            var dirName = Path.GetFileName(@base);

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file);
                var relative = Path.GetRelativePath(@base, path);
                var _relative = relative.StartsWith('.') ? relative[1..] : relative;
                var fullName = Path.GetRelativePath(@base, file);
                var entry = new Entry
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Extension = ext.StartsWith('.') ? ext[1..] : ext,
                    FullName = Path.Combine(dirName, fullName),
                    Path = Path.Combine(dirName, _relative),
                    Source = new()
                    {
                        Path = root,
                        IsVpk = false,
                    }
                };
                packages.AddEntry(entry);
            }

            var dirs = Directory.EnumerateDirectories(path);
            foreach (var dir in dirs)
            {
                AddFolder(packages, root, @base, dir);
            }
        }

        public static void AddAddon(this Packages packages, string addonInfoFile)
        {
            var baseFolder = Path.GetDirectoryName(addonInfoFile);
            var rootFolder = Path.GetDirectoryName(baseFolder);
            AddFolder(packages, rootFolder, baseFolder, baseFolder);
        }
    }
}
