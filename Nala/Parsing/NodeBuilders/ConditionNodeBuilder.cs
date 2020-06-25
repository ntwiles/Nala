using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class ConditionNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence)
        {
            ConditionNode condition = new ConditionNode();

            int openParenPos = 0;
            int closeParenPos = 0;

            for (int i = 0; i < sentence.Count; i++)
            {
                var token = sentence[i];

                switch (token.value)
                {
                    case "(": if (openParenPos == 0) openParenPos = i + 1; break;
                    case ")": if (closeParenPos == 0) closeParenPos = i; break;
                }
            }

            List<NalaToken> betweenParens = sentence.GetRange(openParenPos, closeParenPos - openParenPos);
            condition.expression = (ExpressionNode)(new ExpressionNodeBuilder().BuildNode(betweenParens));

            var parser = new Parser();

            List<NalaToken> afterOpeningBrace = parser.GetNextSentence(closeParenPos + 2, sentence, true);

            // TODO We're not doing anything if the parse fails -- is that okay here? 
            parser.ProcessTokens(afterOpeningBrace, out condition.gotoTrue);

            return condition;
        }
    }
}
