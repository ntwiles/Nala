
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Parsing.Nodes;
using NathanWiles.Nala.Errors;

namespace NathanWiles.Nala.Interpreting
{
    public class Interpreter
    {
        public bool AbortExecution = false;

        public bool Execute(List<ParseNode> parseTree, Scope parentScope)
        {
            var scope = new Scope(parentScope);

            foreach(var node in parseTree)
            { 
                switch (node)
                {
                    case DeclarationNode    decl: interpretDeclaration(decl, scope); break;
                    case AssignmentNode   assign: interpretAssignment(assign, scope); break;
                    case EchoNode           echo: interpretEcho(echo, scope); break;
                    case ReadNode           read: interpretRead(read, scope); break;
                    case ConditionNode condition: interpretConditional(condition, scope); break;
                    case WhileLoopNode      loop: interpretWhileLoop(loop, scope); break;
                    case ClearNode         clear: interpretClear(); break;
                }

                if (AbortExecution) { return false; }
            }

            return true;
        }

        private void interpretDeclaration(DeclarationNode declare, Scope currentScope)
        {
            object initialVal;

            switch (declare.type)
            {
                case PrimitiveType.Int:
                    {
                        if (currentScope.ContainsIdentifier(declare.identifier.name))
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report();
                        else
                        {
                            if (declare.isArray)
                            {
                                int size = ((IntNode)declare.sizeArgument).value;
                                initialVal = new int[size];
                            }
                            else initialVal = 0;
                            currentScope.AddVariable(declare.identifier.name, initialVal);
                        }
                        break;
                    }
                case PrimitiveType.String:
                    {
                        if (currentScope.ContainsIdentifier(declare.identifier.name))
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report();
                        else
                        {
                            if (declare.isArray)
                            {
                                int size = ((IntNode)declare.sizeArgument).value;
                                initialVal = new string[size];
                            }
                            else initialVal = "";
                            currentScope.AddVariable(declare.identifier.name, initialVal);
                        }
                        break;
                    }
                case PrimitiveType.Bool:
                    {
                        if (currentScope.ContainsIdentifier(declare.identifier.name))
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report();
                        else
                        {
                            if (declare.isArray)
                            {
                                int size = ((IntNode)declare.sizeArgument).value;
                                initialVal = new bool[size];
                            }
                            else initialVal = false;
                            currentScope.AddVariable(declare.identifier.name, initialVal);
                        }
                        break;
                    }
            }
        }

        private void interpretAssignment(AssignmentNode assign, Scope currentScope)
        {
            string identifier = assign.identifier.name;
            object resolved = Expressions.ResolveExpression(assign.expression, currentScope);

            if (!currentScope.ContainsIdentifier(identifier))
            {
                new RuntimeError("Use of undeclared identifier \"" + assign.identifier.name + "\".").Report();
                return;
            }

            Operations.DoAssign(currentScope, assign, resolved);
        }

        private void interpretRead(ReadNode read, Scope currentScope)
        {
            string input = Console.ReadLine();

            if (read.identifier == null) return;

            var identifier = read.identifier.name;

            if (currentScope.ContainsIdentifier(read.identifier.name))
            {
                object variable = currentScope.GetValue(read.identifier.name);

                switch (variable)
                {
                    case int i:
                        {
                            int inputInt;

                            if (Int32.TryParse(input, out inputInt))
                            {
                                currentScope.SetValue(identifier,inputInt);
                            }
                            else new RuntimeError("Variable \"" + identifier + "\" of type \"" + variable.GetType() + "\" can not accept value \"" + input + "\".").Report();
                            return;
                        }
                    case string s:
                        {
                            currentScope.SetValue(identifier,input); return;
                        }
                    case bool b:
                        {
                            bool inputBool;

                            if (Boolean.TryParse(input, out inputBool))
                            {
                                currentScope.SetValue(identifier,inputBool);
                            }
                            else new RuntimeError("Variable \"" + identifier + "\" of type \"" + variable.GetType() + "\" can not accept value \"" + input + "\".").Report();
                            return;
                        }
                }
            }
            else
            {
                new RuntimeError("Use of undeclared identifier \"" + read.identifier.name + "\".").Report();  
            }
        }

        private void interpretEcho(EchoNode echo, Scope currentScope)
        {
            ExpressionNode expression = echo.expression;

            object resolution = Expressions.ResolveExpression(expression, currentScope);

            if (resolution != null)
            {
                if (echo.isNewLine) Console.WriteLine(resolution.ToString());
                else Console.Write(resolution.ToString());
            }
        }

        private void interpretClear()
        {
            Console.Clear();
        }

        private void interpretConditional(ConditionNode condition, Scope currentScope)
        {
            bool resolution = Expressions.ResolveRelationalExpression(condition.expression, currentScope);

            List<ParseNode> gotoTrue = condition.gotoTrue;

            if (resolution) Execute(condition.gotoTrue, currentScope);
        }

        private void interpretWhileLoop(WhileLoopNode loop, Scope currentScope)
        {
            bool resolution = Expressions.ResolveRelationalExpression(loop.expression, currentScope);
            List<ParseNode> gotoLoop = loop.gotoLoop;

            while (resolution)
            {
                resolution = Expressions.ResolveRelationalExpression(loop.expression, currentScope);
                if (resolution) Execute(gotoLoop, currentScope);

                if (AbortExecution) { break; }
            }
        }
    }
}
