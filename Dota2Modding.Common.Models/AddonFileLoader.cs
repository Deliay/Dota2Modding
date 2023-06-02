using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models
{
    public class AddonFileLoader : IIncludedFileLoader
    {
        private readonly string folder;

        public AddonFileLoader(string folder)
        {
            this.folder = folder;
        }

        public Stream OpenFile(string filePath)
        {
            return File.OpenRead(Path.Combine(folder, filePath));
        }
    }
}
