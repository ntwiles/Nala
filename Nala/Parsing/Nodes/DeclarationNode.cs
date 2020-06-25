using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.Parsing.Nodes
{
    public class DeclarationNode : ParseNode
    {
        public ParseNode Parent { get; }

        public PrimitiveType type;
        public IdentifierNode identifier;

        public bool isArray;
        public ParseNode sizeArgument;
    }
}
