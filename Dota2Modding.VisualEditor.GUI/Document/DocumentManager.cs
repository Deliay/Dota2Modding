using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Document
{
    public class DocumentManager : ObservableCollection<IDocumentPage>
    {
        private readonly Dictionary<string, IDocumentPage> pages = new();

        public ValueTask Register(IDocumentPage page)
        {
            pages.Add(page.Id, page);

            return ValueTask.CompletedTask;
        }
    }
}
