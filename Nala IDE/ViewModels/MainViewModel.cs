using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NathanWiles.NalaIDE.NalaConsole;
using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class MainViewModel
    {
        // Menu Bar
        public FileMenuViewModel FileMenu { get; set; }
        public EditMenuViewModel EditMenu { get; set; }

        public HelpMenuViewModel HelpMenu { get; set; }

        public CodeEditorViewModel Editor { get; set; }

        public TabViewModel Tabs { get; set; }
        public HierarchyViewModel Hierarchy { get; set; }
        //public NalaConsoleViewModel NalaConsole { get; set; }

        public MainViewModel()
        {
            var newfile = new DocumentModel();

            Tabs = new TabViewModel();

            FileMenu = new FileMenuViewModel(Tabs.Documents);
            EditMenu = new EditMenuViewModel();
            HelpMenu = new HelpMenuViewModel();
            Editor = new CodeEditorViewModel(Tabs.Documents);

            Hierarchy = new HierarchyViewModel(Tabs.Documents);
            //NalaConsole = new NalaConsoleViewModel();
        }

    }
}
