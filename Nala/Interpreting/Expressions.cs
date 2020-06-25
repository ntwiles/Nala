
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Errors;

namespace NathanWiles.Nala.Interpreting
{
    public static class Expressions
    {
        public static object ResolveExpression(ExpressionNode expression, Scope scope)
        {
            if (expression.isRelational) return ResolveRelationalExpression(expression,scope);
            else return ResolveArithExpression(expression, scope);
        } 

        public static bool ResolveRelationalExpression(ExpressionNode expression, Scope scope)
        {
            var nodes = expression.elements;

            if (nodes.Count == 0)
            {
                new RuntimeError("Relational expression requires at least one token.").Report();
                return false;
            }
            else if (nodes.Count == 1)
            {
                var resolved = ResolveOperand(nodes[0],scope);

                if (!(resolved is bool))
                {
                    new RuntimeError("The resolved expression is not a boolean value.").Report();
                    return false;
                }
                else return (bool)ResolveOperand(nodes[0],scope);
            }

            else
            {
                List<ParseNode> leftExpression, rightExpression;

                int operatorPos = 0;

                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];

                    if (node is OperatorNode && TokenLookups.RelationOperators.Contains(((OperatorNode)node).symbol))
                    {
                        operatorPos = i;
                    }
                }

                leftExpression = nodes.GetRange(0, operatorPos);
                rightExpression = nodes.GetRange(operatorPos + 1, nodes.Count - operatorPos - 1);

                object leftResolved = ResolveOperation(leftExpression, scope);
                object rightResolved = ResolveOperation(rightExpression, scope);

                OperatorNode @operator = (OperatorNode)nodes[operatorPos];

                if (!(@operator is OperatorNode)) { new RuntimeError("Malformed operation.").Report(); return false; }

                return Operations.DoComparison(@operator.symbol, leftResolved, rightResolved);
            }
        }

        public static object ResolveArithExpression(ExpressionNode expression, Scope scope)
        {
            // TODO: Do we really need this if/else? We can probably rewrite this to always work.
            if (expression.elements.Count == 1) { return ResolveOperand(expression.elements[0],scope); }
            else { return ResolveOperation(expression.elements,scope); }
        }

        // TODO: We don't want all the variables, we want to accept the relavent scope instead.
        public static object ResolveOperation(List<ParseNode> elements, Scope scope)
        {
            if (elements.Count == 0)
            {
                new RuntimeError("Operation requires at least one operand.").Report();
                return null;
            }

            object resolution = ResolveOperand(elements[0],scope);
            if (resolution == null) { return null; }

            Type expressionType = resolution.GetType();

            for (int i = 0; i < elements.Count; i += 2)
            {
                if (i >= elements.Count - 1) return resolution;

                object rightOperand = ResolveOperand(elements[i + 2],scope);
                if (rightOperand == null) { return null; }

                OperatorNode @operator = (OperatorNode)elements[i + 1];

                if (!(@operator is OperatorNode)) { new RuntimeError("Malformed operation.").Report(); return null; }

                if (rightOperand.GetType() != expressionType)
                {
                    new RuntimeError("Operatand type mismatch between \"" + expressionType + "\" and \"" + rightOperand.GetType() + "\".").Report();
                    return null;
                }

                switch (@operator.symbol)
                {
                    case "+": resolution = Operations.DoAdd(resolution, rightOperand); break;
                    case "*": resolution = Operations.DoMultiply(resolution, rightOperand); break;
                    case "-": resolution = Operations.DoSubtract(resolution, rightOperand); break;
                    case "/": resolution = Operations.DoDivide(resolution, rightOperand); break;
                }
            }

            return resolution;
        }

        public static object ResolveOperand(ParseNode expressionElem, Scope scope)
        {
            switch (expressionElem)
            {
                case IdentifierNode identifier:
                    {
                        if (scope.ContainsIdentifier(identifier.name))
                        {
                            object variable = scope.GetValue(identifier.name);

                            if (identifier.hasIndexer)
                            {
                                switch (variable)
                                {
                                    case int[] i:
                                        {
                                            int[] array = (int[])variable;

                                            if (identifier.index is IdentifierNode)
                                            {
                                                return array[(int)ResolveOperand(identifier.index,scope)];
                                            }
                                            else
                                            {
                                                return array[((IntNode)identifier.index).value];
                                            }
                                        }
                                    case string[] s:
                                        {
                                            string[] array = (string[])variable;

                                            if (identifier.index is IdentifierNode)
                                            {
                                                return array[(int)ResolveOperand(identifier.index, scope)];
                                            }
                                            else
                                            {
                                                return array[((IntNode)identifier.index).value];
                                            }
                                        }
                                    case bool[] b:
                                        {
                                            bool[] array = (bool[])variable;

                                            if (identifier.index is IdentifierNode)
                                            {
                                                return array[(int)ResolveOperand(identifier.index, scope)];
                                            }
                                            else
                                            {
                                                return array[((IntNode)identifier.index).value];
                                            }
                                        }
                                    default:
                                        {
                                            throw new NotImplementedException(variable.GetType().ToString());
                                        }
                                }
                            }
                            else
                            {
                                return variable;
                            }
                        }
                        else
                        {
                            new RuntimeError("Use of undeclared identifier: " + identifier.name).Report();
                            return null;
                        }
                    }

                case IntNode intNode: return intNode.value;
                case StringNode strNode: return strNode.value;
                case BoolNode boolNode: return boolNode.value;
            }

            return null;
        }
    }
}
