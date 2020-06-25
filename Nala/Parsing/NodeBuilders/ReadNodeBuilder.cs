using System;
using System.Collections.Generic;
using System.Text;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class ReadNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence, IIOContext ioContext)
        {
            ReadNode read = new ReadNode();

            if (sentence[1].type == TokenType.Identifier)
            {
                read.identifier = new IdentifierNode(sentence[1].value);
            }

            return read;
        }
    }
}
