using System;
using System.Collections.Generic;
using System.Text;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class ClearNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence, IIOContext ioContext)
        {
            return new ClearNode();
        }
    }
}
