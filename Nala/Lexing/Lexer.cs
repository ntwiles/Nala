
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NathanWiles.Nala.Errors;


namespace NathanWiles.Nala.Lexing
{
    public class Lexer
    {
        //private List<NalaToken> tokens;
        public bool AbortLexing;

        public Lexer()
        {
            AbortLexing = false;
        }

        public bool TryProcessCode(List<string> nalaCodeLines, out List<NalaToken> tokens)
        {
            List<char> emptyChars = new List<char> { '\t', '\n', '\r', ' ' };
            List<char> operatorChars = new List<char> { '+', '-', '*', '/', '=', '>', '<', '!', ';',',','(',')','[',']'};
            List<char> loneleyOperatorChars = new List<char> { '(', ')', '[', ']', };

            tokens = new List<NalaToken>();

            string token = "";

            for (int il = 0; il < nalaCodeLines.Count; il++)
            {
                var nalaLine = nalaCodeLines[il];

                for (int ic = 0; ic < nalaLine.Length; ic++)
                {
                    char @char = nalaLine[ic];

                    bool charIsWhitespace = emptyChars.Contains(@char);
                    bool charIsOperator = operatorChars.Contains(@char);

                    // Ignore whitespace.
                    if (charIsWhitespace)
                    {
                        if (token != "")
                        {
                            addToken(tokens,token, il, ic);
                            token = "";
                        }

                        continue;
                    }

                    // This is the beginning of a comment.
                    if (@char == '/' && ic < nalaLine.Length - 1 && nalaLine[ic + 1] == '/')
                    {
                        break;
                    }

                    //This is the beginning of a string.
                    if (@char == '"')
                    {
                        int stringClosePos = nalaLine.IndexOf('"', ic + 1);

                        if (stringClosePos == -1)
                        {
                            new LexerError(il, ic, "All strings must have a closing symbol (\").").Report();
                            return false;
                        }

                        string stringChars = nalaLine.Substring(ic + 1, stringClosePos - (ic + 1));
                        addToken(tokens, TokenType.StringLiteral, stringChars,il,ic);

                        ic = stringClosePos;
                        continue;
                    }

                    if (charIsOperator)
                    {
                        //We set it to 'z' simply so that 'nextCharIsOperator' will resolve false.
                        char nextChar = ic < nalaLine.Length - 1 ? nalaLine[ic + 1] : 'z';
                        bool nextCharIsOperator = operatorChars.Contains(nextChar);

                        addToken(tokens,token, il, ic);

                        //The following operators should exist in their own token even if adjacent to another operator.
                        if (loneleyOperatorChars.Contains(@char))
                        {
                            addToken(tokens,"" + @char, il, ic);
                            token = "";
                            continue;
                        }
                        else if (nextCharIsOperator)
                        {
                            string twoCharOperator = "" + @char + nextChar;
                            addToken(tokens,twoCharOperator, il, ic);
                            token = "";

                            ic += 1;
                            continue;
                        }
                        else
                        {
                            addToken(tokens,""+@char, il, ic);
                            token = "";
                            continue;
                        }
                    }

                    //If it's none of the above, it's build the token normally.
                    token += @char;
                   
                    if (AbortLexing) { return false; }
                }

                addToken(tokens,token,il,0);
                token = "";

                if (AbortLexing) { return false; }
            }

            return true;
        }
        
        private void addToken(List<NalaToken> tokens, TokenType tokenType, string tokenVal, int line, int column)
        {
            tokens.Add(new NalaToken(tokenType, tokenVal, line, column));
        }

        private void addToken(List<NalaToken> tokens, string tokenVal, int line, int column)
        {
            if (tokenVal == "") return;

            int tokenInt;
            TokenType tokenType = TokenType.Unknown;

            // We have a string
            char[] tokenChars = tokenVal.ToCharArray();
            if (tokenChars[0] == '"' && tokenChars[tokenChars.Length - 1] == '"')
            {
                tokenType = TokenType.StringLiteral;
                tokenVal = tokenVal.Replace("\"", "");
            }

            // We have an operator.
            else if (TokenLookups.Operators.Contains(tokenVal))
            {
                tokenType = TokenType.Operator;
            }

            // We have a primitive (int, float, char, etc.)
            else if (TokenLookups.Primitives.Contains(tokenVal))
            {
                tokenType = TokenType.Primitive;
            }

            // We have a keyword.
            else if (TokenLookups.Keywords.Contains(tokenVal))
            {
                tokenType = TokenType.Keyword;
            }

            // We have an integer literal.
            else if (int.TryParse(tokenVal, out tokenInt))
            {
                tokenType = TokenType.IntLiteral;
            }

            // We have a punctuator.
            else if (TokenLookups.Punctuators.Contains(tokenVal))
            {
                tokenType = TokenType.Punctuator;
            }

            else if (tokenVal == "true" || tokenVal == "false")
            {
                tokenType = TokenType.BoolLiteral;
            }

            // The only thing left for this to be is a variable identifier.
            // TODO: While this is true, we need rules for what can be an identifier.
            // Good rules: Can contain letters, numbers, -'s, and _'s, and must not begin with a number.
            // Modify the regex to reflect this rule.
            else
            {
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                if (r.IsMatch(tokenVal)) tokenType = TokenType.Identifier;
                else new LexerError(line, column, "Invalid identifier \"" + tokenVal + "\". Identifiers can be comprised only of alphanumeric characters.").Report();
            }

            addToken(tokens, tokenType, tokenVal, line, column);
        }
    }

    public class NalaToken
    {
        public TokenType type;
        public string value;

        public int line;
        public int column;

        public NalaToken(TokenType type, string value, int line, int column)
        {
            this.type = type;
            this.value = value;
            this.line = line;
            this.column = column;
        }
    }

    public enum TokenType
    {
        Unknown,

        Punctuator,
        Operator,

        Parenth,

        Primitive,
        Keyword,
        Identifier,

        IntLiteral,
        StringLiteral,
        BoolLiteral
    }
}
