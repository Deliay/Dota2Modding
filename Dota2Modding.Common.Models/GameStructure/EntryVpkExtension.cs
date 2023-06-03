using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveResourceFormat;
using ValveResourceFormat.Blocks;
using ValveResourceFormat.ResourceTypes;

namespace Dota2Modding.Common.Models.GameStructure
{
    public static class EntryVpkExtension
    {
        private static byte[] ExtractBlock(Block block)
        {
            if (block is Texture tex)
            {
                using var bitmap = tex.GenerateBitmap();
                return bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
            }

            throw new InvalidDataException();
        }

        private static byte[] ExtractFromValveFormat(byte[] valveFormat)
        {
            using var ms = new MemoryStream(valveFormat);

            var res = new Resource();
            res.Read(ms);
            var data = new ResourceData();

            var extracted = ExtractBlock(res.DataBlock);
            return extracted;
        }

        public static byte[] LoadResourceData(this Entry entry, Packages packages)
        {

            if (entry.Source.IsVpk)
            {
                var vpk = packages.GetVpk(entry);
                var ent = packages.GetPackageEntry(entry);

                vpk.ReadEntry(ent, out var raw);

                return ExtractFromValveFormat(raw);
            }

            var plainRaw = File.ReadAllBytes(entry.GetFullPath());
            if (entry.Extension.EndsWith("_c"))
            {
                return ExtractFromValveFormat(plainRaw);
            }

            return plainRaw;
        }
    }
}
