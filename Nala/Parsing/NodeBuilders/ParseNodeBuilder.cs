
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public abstract class ParseNodeBuilder
    {
        public abstract ParseNode BuildNode(List<NalaToken> sentence);
    }
}


