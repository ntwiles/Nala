
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.Win32;

namespace NathanWiles.NalaIDE.Models
{
    public class DocumentsModel : ObservableCollection<DocumentModel>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DocumentModel _currentDocument;

        public DocumentModel CurrentDocument
        {
            get { return _currentDocument; }
            set { OnPropertyChanged(ref _currentDocument, value); }   
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

        public void OpenFile(string path)
        {
            var lines = File.ReadAllLines(path).ToList();

            var document = new DocumentModel(lines);
            document.FilePath = path;
            document.FileName = Path.GetFileName(path);

            Add(document);

            CurrentDocument = document;
        }

        public void OpenFile(OpenFileDialog openFileDialog)
        {
            OpenFile(openFileDialog.FileName);
        }
    }
}
