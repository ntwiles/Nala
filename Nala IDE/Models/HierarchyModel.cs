using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NathanWiles.NalaIDE.Models
{
    public class HierarchyModel : ObservableCollection<DocumentModel>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DocumentModel _selected;

        public DocumentModel Selected
        {
            get { return _selected; }
            set { OnPropertyChanged(ref _selected, value); }
        }

        public HierarchyModel()
        {
            string path = "D:/OneDrive/Development/nala/";
            string[] files = Directory.GetFiles(path, "*.nl");

            foreach(string file in files)
            {
                var document = new DocumentModel();
                document.FileName = Path.GetFileName(file);
                document.FilePath = file;

                Add(document);
            }
        }

        public void OnPropertyChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            property = value;
            var handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
