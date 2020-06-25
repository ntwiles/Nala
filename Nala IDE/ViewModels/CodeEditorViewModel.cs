
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class CodeEditorViewModel
    {
        public DocumentsModel Documents { get; set; }

        public CodeEditorViewModel(DocumentsModel documents)
        {
            Documents = documents;
        }
    }
}
