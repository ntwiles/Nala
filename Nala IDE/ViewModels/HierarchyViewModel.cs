
using System;
using System.Windows.Input;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class HierarchyViewModel
    {
        public ICommand OpenFileCommand { get; }

        public HierarchyModel Hierarchy { get; set; }

        private DocumentsModel _documents;

        public HierarchyViewModel(DocumentsModel Documents)
        {
            _documents = Documents;

            OpenFileCommand = new RelayCommand(OpenFile);

            Hierarchy = new HierarchyModel();
        }

        public void OpenFile()
        {
            _documents.OpenFile(Hierarchy.Selected.FilePath);
        }
    }
}
