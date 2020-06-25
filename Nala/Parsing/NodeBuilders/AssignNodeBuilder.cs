using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing.Nodes;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class AssignNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence)
        {
            AssignmentNode assign = new AssignmentNode();

            // Identifier of variable to which we're assigning a value.
            assign.identifier = new IdentifierNode(sentence[0].value);

            List<NalaToken> afterOperator = null;

            // We're assigning to a non-arrayed variable.
            if (TokenLookups.AssignOperators.Contains(sentence[1].value))
            {
                assign.@operator = new OperatorNode(sentence[1].value);

                afterOperator = sentence.GetRange(2, sentence.Count - 2);
            }

            // We're assigning to an array index.
            else if (TokenLookups.AssignOperators.Contains(sentence[4].value))
            {
                assign.isArray = true;
                assign.indexArgument = new IntNode(Int32.Parse(sentence[2].value));
                assign.@operator = new OperatorNode(sentence[4].value);

                afterOperator = sentence.GetRange(5, sentence.Count - 5);
            }

            assign.expression = (ExpressionNode)(new ExpressionNodeBuilder().BuildNode(afterOperator));

            return assign;
        }
    }
}
