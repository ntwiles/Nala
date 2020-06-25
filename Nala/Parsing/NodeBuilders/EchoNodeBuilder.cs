using System;
using System.Collections.Generic;
using System.Text;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class EchoNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence, IIOContext ioContext)
        {
            EchoNode echo = new EchoNode();

            List<NalaToken> afterEcho = sentence.GetRange(1, sentence.Count - 1);

            echo.expression = (ExpressionNode)(new ExpressionNodeBuilder().BuildNode(afterEcho, ioContext));
            echo.isNewLine = sentence[0].value == "echoline";

            return echo;
        }
    }
}
