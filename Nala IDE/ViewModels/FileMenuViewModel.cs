using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Win32;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Interpreting;

using NathanWiles.NalaIDE.Models;
using NathanWiles.NalaIDE.NalaConsole;

namespace NathanWiles.NalaIDE.ViewModels
{
    public class FileMenuViewModel
    {
        private DocumentsModel _documents;
        private NalaConsoleModel _console;
        private NalaConsoleWindow _consoleWindow;

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand OpenFolderCommand { get; }
        public ICommand RunCommand { get; }

        public FileMenuViewModel(DocumentsModel documents)
        {
            _documents = documents;

            NewCommand = new RelayCommand(NewFile);
            SaveCommand = new RelayCommand(SaveFile);
            SaveAsCommand = new RelayCommand(SaveFileAs);
            OpenFileCommand = new RelayCommand(OpenFile);
            OpenFolderCommand = new RelayCommand(OpenFolder);
            RunCommand = new RelayCommand(ExecuteFile);

            _consoleWindow = new NalaConsoleWindow();

            TokenLookups.Init();
        }

        public void NewFile()
        {
            var document = new DocumentModel();
            _documents.Add(document);
        }

        private void SaveFile()
        {
            var document = _documents.CurrentDocument;

            if (document.FilePath == null) SaveFileAs();
            else File.WriteAllLines(document.FilePath, document);
        }

        private void SaveFileAs()
        {
            var document = _documents.CurrentDocument;

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Nala Source (*.nl)|*.nl";
            if (saveFileDialog.ShowDialog() == true)
            {
                document.FilePath = saveFileDialog.FileName;
                document.FileName = saveFileDialog.SafeFileName;
                SaveFile();
            }
        }

        private void OpenFile()
        {
            DocumentModel document;
            bool createdNew = false;

            //If our current file is empty and unsaved, let's just replace it instead of opening a new one.
            if (_documents.CurrentDocument.isEmpty)
            {
                document = _documents.CurrentDocument;
            }
            else
            {
                document = new DocumentModel();
                createdNew = true;
            }

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Nala Source (*.nl)|*.nl";

            if (openFileDialog.ShowDialog() == true)
            {
                _documents.OpenFile(openFileDialog.FileName);
            }
        }

        private void OpenFolder()
        {
            throw new NotImplementedException();
        }

        private void ExecuteFile()
        {
            SaveFile();

            var document = _documents.CurrentDocument;

            string binPath = @"D:\Development\C#\Nala\Nala\bin\Debug\netcoreapp3.1";
            string nala = Path.Combine(binPath,"nala");
            string target = document.FileName;

            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Path.GetDirectoryName(document.FilePath);
            startInfo.FileName = nala;
            startInfo.ArgumentList.Add(target);


            Process.Start(startInfo);

            /*
            List<string> lines = document.Text.Split(new[] { '\r', '\n' }).ToList();
            List<NalaToken> tokens = Lexer.ProcessCode(lines);
            List<ParseNode> nodes = Parser.ProcessTokens(tokens);
            Interpreter.Execute(nodes);

            _consoleWindow.Title = document.FileName;
            _consoleWindow.Show();
            */
        }
    }
}
