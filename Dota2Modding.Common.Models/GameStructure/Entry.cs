using Dota2Modding.Common.Models.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Entry : IFullContextSearchable, IIdentity
    {
        public Source Source { get; set; }
        public string FullName { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public ulong Id { get; set; }

        public string GetContainsText()
        {
            return $"{FullName} {Name}.{Extension} {Extension} {Source.Path} {Path}";
        }
    }
}
