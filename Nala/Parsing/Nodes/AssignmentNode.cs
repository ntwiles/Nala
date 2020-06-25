using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.Parsing.Nodes
{
    public class AssignmentNode : ParseNode
    {
        // Variable
        public IdentifierNode identifier;
        public bool isArray;
        public ParseNode indexArgument;

        // Operator
        public OperatorNode @operator;

        // Expression
        public ExpressionNode expression;
    }
}
