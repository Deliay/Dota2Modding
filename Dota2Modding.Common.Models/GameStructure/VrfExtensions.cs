﻿using DBreeze.Transactions;
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
            
            var filename = $"{packageEntry.FileName}.{packageEntry.TypeName}";
            var entry = new Entry
            {
                FullName = $"{packageEntry.DirectoryName}{Package.DirectorySeparatorChar}{filename}",
                Name = packageEntry.FileName,
                Extension = packageEntry.TypeName,
                Path = packageEntry.DirectoryName,
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
    }
}