using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class ConditionParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence)
        {
            var token = sentence[0];
            if (token.value == "if")
            {
                return IsProper(sentence);
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence)
        {
            int openParenPos = 0, closeParenPos = 0;

            for (int i = 0; i < sentence.Count; i++)
            {
                var token = sentence[i];

                switch (token.value)
                {
                    case "(": openParenPos = i + 1; break;
                    case ")": closeParenPos = i; break;
                }
            }

            List<NalaToken> betweenParens = sentence.GetRange(openParenPos, closeParenPos - openParenPos);

            if (!(new ExpressParseRule().Matches(betweenParens))) return false;

            return true;
        }
    }
}
