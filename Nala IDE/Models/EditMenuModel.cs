using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.NalaIDE.Models
{
    public class EditMenuModel : ObservableObject
    {
        private NalaEditorTheme _theme;

        public NalaEditorTheme Theme
        {
            get { return _theme; }
            set { OnPropertyChanged(ref _theme, value); }
        }
    }

    public enum NalaEditorTheme
    {
        Dark,
        Light
    }
}
