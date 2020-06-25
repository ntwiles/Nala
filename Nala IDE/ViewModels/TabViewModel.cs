using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class TabViewModel
    {
        public ICommand ChangeTabCommand { get; }

        public DocumentsModel Documents { get; private set; }

        public TabViewModel()
        {
            ChangeTabCommand = new RelayCommand(ChangeTab);

            Documents = new DocumentsModel();
            Documents.Add(new DocumentModel());
            Documents.CurrentDocument = Documents[0];
        }

        private void ChangeTab()
        {
            throw new NotImplementedException();
        }
    }
}
