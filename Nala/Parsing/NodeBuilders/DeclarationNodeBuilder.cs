using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing.Nodes;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class DeclarationNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> sentence)
        {
            DeclarationNode declaration = new DeclarationNode();

            if (sentence.Count == 3)
            {
                switch (sentence[0].value)
                {
                    case "int": declaration.type = PrimitiveType.Int; break;
                    case "string": declaration.type = PrimitiveType.String; break;
                    case "bool": declaration.type = PrimitiveType.Bool; break;
                }

                declaration.identifier = new IdentifierNode(sentence[1].value);
            }

            else if (sentence.Count == 6)
            {
                declaration.isArray = true;

                //Get the identifier.
                var identifier = new IdentifierNode(sentence[4].value);
                declaration.identifier = identifier;

                //Get the type.
                switch (sentence[0].value)
                {
                    case "int": declaration.type = PrimitiveType.Int; break;
                    case "string": declaration.type = PrimitiveType.String; break;
                    case "bool": declaration.type = PrimitiveType.Bool; break;
                }

                //Get the indexer argument.

                switch (sentence[2].type)
                {
                    case TokenType.IntLiteral: declaration.sizeArgument = new IntNode(Int32.Parse(sentence[2].value)); break;
                    case TokenType.Identifier: declaration.sizeArgument = new IdentifierNode(sentence[2].value); break;
                }
            }

            return declaration;
        }
    }
}
