using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class HelpMenuViewModel
    {
        public ICommand AboutCommand { get; }

        public HelpMenuViewModel()
        {
            AboutCommand = new RelayCommand(DisplayAbout);
        }

        private void DisplayAbout()
        {
            // We will about help dialog.
            throw new NotImplementedException();
        }
    }
}
