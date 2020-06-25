using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using NathanWiles.Nala.Lexing;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.CodeEditor
{
    public class SyntaxHighlighter
    {
        private Color _colorKeyword, _colorComment, _colorType;
        private Color _colorPurple, _colorOrange, _colorGreen;

        public SyntaxHighlighter()
        {

            _colorPurple = Color.FromArgb(255, 168, 95, 255);
            _colorOrange = Color.FromArgb(255, 255, 194, 145);
            _colorGreen = Color.FromArgb(255, 145, 255, 163);

            _colorKeyword = _colorPurple;
            _colorType = _colorOrange;
            _colorComment = _colorGreen;
        }

        public List<string> SplitToWordsAndWhitespace(DocumentModel lines)
        {
            var words = new List<string>();

            string currentWord = "";

            for (int ir = 0; ir < lines.Count; ir++)
            {
                string line = lines[ir];

                for (int ic = 0; ic < line.Length; ic++)
                {
                    char character = line[ic];

                    if (Char.IsWhiteSpace(character))
                    {
                        if (currentWord != "")
                        {
                            words.Add(currentWord);
                            currentWord = "";
                        }

                        words.Add("" + character);
                    }
                    else
                    {
                        currentWord += character;
                    }
                }

                words.Add(currentWord);
                words.Add("\n");
                currentWord = "";
            }

            return words;
        }

        public SolidColorBrush GetBrush(string word)
        {
            if (TokenLookups.Keywords.Contains(word)) return new SolidColorBrush(_colorKeyword);
            if (TokenLookups.Primitives.Contains(word)) return new SolidColorBrush(_colorType);
            
            return Brushes.White;
        }
    }
}
