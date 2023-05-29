using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public interface FolderView : IEnumerable<FolderView>
    {
        public FolderItemType ItemType { get; }

        public string DisplayName { get; }

    }
}
