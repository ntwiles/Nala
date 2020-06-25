
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NathanWiles.NalaIDE.Models
{
    public class NalaConsoleModel : ObservableCollection<DocumentModel>, INotifyPropertyChanged
    {
        public List<string> Output
        {
            get; set;
        }

        public NalaConsoleModel()
        {
            Output = new List<string>();
        }
    }
}
