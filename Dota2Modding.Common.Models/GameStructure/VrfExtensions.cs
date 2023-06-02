using DBreeze.Transactions;
using SteamDatabase.ValvePak;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public static class VrfExtensions
    {
        public static void AddPackageEntry(this Packages workspace, Package package, PackageEntry packageEntry)
        {
            var dir = Path.GetDirectoryName(package.FileName);
            var dirName = Path.GetFileName(dir);
            var appendDir = $"{dirName}{Package.DirectorySeparatorChar}";
            var filename = $"{packageEntry.FileName}.{packageEntry.TypeName}";
            var entry = new Entry
            {
                FullName = $"{appendDir}{packageEntry.DirectoryName}{Package.DirectorySeparatorChar}{filename}",
                Name = packageEntry.FileName,
                Extension = packageEntry.TypeName,
                Path = $"{appendDir}{packageEntry.DirectoryName}",
                Source  = new Source
                {
                    Path = package.FileName,
                    IsVpk = true,
                },
            };
            workspace.AddEntry(entry);
            workspace.AssociateExtraData(entry, package);
            workspace.AssociateExtraData(entry, packageEntry);

        }

        public static void AddPackage(this Packages workspace, Package package)
        {
            ulong count = 0;
            ulong all = 0;
            foreach (var (key, value) in package.Entries)
            {
                foreach (var item in value)
                {
                    count += 1;
                    workspace.AddPackageEntry(package, item);

                    if (count > 5000)
                    {
                        all += count;
                        count = 0;
                    }
                }
            }
        }

        public static void AddVpk(this Packages workspace, string path)
        {
            var pak = new Package();
            pak.Read(path);
            workspace.AssociateDisposable(pak);

            workspace.AddPackage(pak);
        }

        public static void AddVpk(this Packages workspace, Stream stream)
        {
            var pak = new Package();
            pak.Read(stream);
            workspace.AssociateDisposable(pak);

            workspace.AddPackage(pak);
        }

        public static Package GetVpk(this Packages workspace, Entry entry)
        {
            return workspace.GetExtraData<Package>(entry);
        }

        public static PackageEntry GetPackageEntry(this Packages workspace, Entry entry)
        {
            return workspace.GetExtraData<PackageEntry>(entry);
        }
    }
}
