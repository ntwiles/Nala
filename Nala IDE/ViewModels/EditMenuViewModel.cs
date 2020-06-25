
using System;
using System.Windows.Input;

using NathanWiles.NalaIDE.Models;
using NathanWiles.NalaIDE.NalaConsole;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class EditMenuViewModel
    {
        public ICommand PreferencesCommand { get; }

        public EditMenuViewModel()
        {
            PreferencesCommand = new RelayCommand(EditPreferences);
        }

        public void EditPreferences()
        {
            throw new NotImplementedException();
        }
    }
}
