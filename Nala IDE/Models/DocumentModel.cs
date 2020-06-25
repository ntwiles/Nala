using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NathanWiles.NalaIDE.Models
{
    public class DocumentModel : ObservableCollection<string>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { OnPropertyChanged(ref _filePath, value); }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { OnPropertyChanged(ref _fileName, value); }
        }

        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(FileName) ||
                    string.IsNullOrEmpty(FilePath))
                {
                    return true;
                }

                return false;
            }
        }

        public DocumentModel(List<string> lines)
        {
            foreach(string line in lines)
            {
                Add(line);
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

        public DocumentModel()
        {
            FileName = "Untitled";
        }
    }
}
