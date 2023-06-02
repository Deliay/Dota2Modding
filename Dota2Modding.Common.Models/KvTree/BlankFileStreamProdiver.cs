using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class BlankFileStreamProdiver : IIncludedFileLoader
    {
        public static readonly BlankFileStreamProdiver Instance = new();

        private BlankFileStreamProdiver() { }

        public Stream OpenFile(string filePath)
        {
            return Stream.Null;
        }
    }
}
