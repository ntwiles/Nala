using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.CodeEditor
{
    public class CursorPosition
    {
        private int _x, _y;

        public int X 
        { 
            get { return _x; }
            set
            {
                _x = value;

                if (_x < 0)
                {
                    if (_y > 0)
                    {
                        _y--;
                        _x = _document[_y].Length;
                    }
                    else
                    {
                        _x = 0;
                    }
                }

                if (_x > _document[_y].Length)
                {
                    if (_document.Count > _y + 1)
                    {
                        _y++;
                        _x = 0;

                    }
                    else
                    {
                        _x = _document[_y].Length;
                    }
                }
            }
        }
        public int Y 
        { 
            get { return _y; }
            set
            {
                _y = value;

                if (_x > _document[_y].Length) { _x = _document[_y].Length; }

                if (_y < 0)
                {
                    _y = 0;
                }

                if (_y >= _document.Count)
                {
                    _y = _document.Count - 1;
                }
            }
        }

        private DocumentModel _document;

        public CursorPosition(DocumentModel document)
        {
            _document = document;
        }
    }
}
