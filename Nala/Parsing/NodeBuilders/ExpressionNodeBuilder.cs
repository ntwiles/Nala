using System;
using System.Collections.Generic;
using System.Text;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.NodeBuilders
{
    public class ExpressionNodeBuilder : ParseNodeBuilder
    {
        public override ParseNode BuildNode(List<NalaToken> expression, IIOContext ioContext)
        {
            ExpressionNode expressionNode = new ExpressionNode();

            for (int i = 0; i < expression.Count; i++)
            {
                var token = expression[i];

                switch (token.type)
                {
                    case TokenType.Identifier:
                        {
                            var identifier = new IdentifierNode(token.value);

                            expressionNode.elements.Add(identifier);

                            if (i < expression.Count - 1)
                            {
                                var nextToken = expression[i + 1];

                                if (nextToken.value == "[")
                                {
                                    identifier.hasIndexer = true;
                                    var indexer = expression[i + 2];

                                    if (indexer.type == TokenType.Identifier)
                                    {
                                        identifier.index = new IdentifierNode(indexer.value);
                                    }
                                    else
                                    {
                                        identifier.index = new IntNode(Int32.Parse(indexer.value));
                                    }

                                    i += 3;
                                }
                            }

                            break;
                        }
                    case TokenType.Operator:
                        {
                            expressionNode.elements.Add(new OperatorNode(token.value));
                            break;
                        }
                    case TokenType.IntLiteral:
                        {
                            expressionNode.elements.Add(new IntNode(Int32.Parse(token.value)));
                            break;
                        }
                    case TokenType.StringLiteral:
                        {
                            expressionNode.elements.Add(new StringNode(token.value));
                            break;
                        }
                    case TokenType.BoolLiteral:
                        {
                            expressionNode.elements.Add(new BoolNode(bool.Parse(token.value)));
                            break;
                        }
                }
            }

            return expressionNode;
        }
    }
}
