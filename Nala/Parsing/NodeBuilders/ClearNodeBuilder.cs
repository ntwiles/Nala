using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class ClearNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence)
        {
            return new ClearNode();
        }
    }
}
