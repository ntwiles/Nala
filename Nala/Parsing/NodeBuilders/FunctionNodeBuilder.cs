using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class FunctionNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence)
        {
            FunctionNode function = new FunctionNode();


            return function;
        }
    }
}
