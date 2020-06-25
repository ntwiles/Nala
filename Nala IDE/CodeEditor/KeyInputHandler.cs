
using System;
using System.Windows.Input;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.CodeEditor
{
    public class KeyInputHandler
    {
        public bool HandleKeyDown(DocumentModel document, CursorPosition cursorPosition, Key key)
        {
            var line = document[cursorPosition.Y];

            switch (key)
            {
                case Key.Back:
                    {
                        string leftSection = line.Substring(0, cursorPosition.X);

                        if (leftSection.Length > 0)
                        {
                            cursorPosition.X--;
                            string temp = document[cursorPosition.Y];
                            document[cursorPosition.Y] = temp.Remove(cursorPosition.X, 1);

                        }
                        else if (cursorPosition.Y > 0)
                        {
                            cursorPosition.X--;
                        }

                        return true;
                    }
                case Key.Enter:
                    {
                        string leftSection = line.Substring(0, cursorPosition.X);
                        string rightSection = line.Substring(cursorPosition.X);

                        document[cursorPosition.Y] = leftSection;
                        document.Insert(cursorPosition.Y + 1, rightSection);
                        cursorPosition.Y++;
                        cursorPosition.X = 0;
                        return true;
                    }
                case Key.Tab: line += '\t'; return true;
                case Key.Left: cursorPosition.X--; return true;
                case Key.Right: cursorPosition.X++; return true;
                case Key.Up: cursorPosition.Y--; return true;
                case Key.Down: cursorPosition.Y++; return true;
                default:return false;
            }
        }
    }
}
