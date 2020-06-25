
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Parsing.Nodes;
using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;

namespace NathanWiles.Nala.Interpreting
{
    public class Interpreter
    {
        private IIOContext ioContext;

        public bool AbortExecution = false;

        public Interpreter(IIOContext ioContext)
        {
            this.ioContext = ioContext;
        }

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
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report(ioContext);
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
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report(ioContext);
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
                            new RuntimeError("Identifier \"" + declare.identifier.name + "\" has already been declared.").Report(ioContext);
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
            object resolved = Expressions.ResolveExpression(assign.expression, currentScope, ioContext);

            if (!currentScope.ContainsIdentifier(identifier))
            {
                new RuntimeError("Use of undeclared identifier \"" + assign.identifier.name + "\".").Report(ioContext);
                return;
            }

            Operations.DoAssign(currentScope, assign, resolved, ioContext);
        }

        private void interpretRead(ReadNode read, Scope currentScope)
        {
            string input = ioContext.ReadLine();

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
                            else new RuntimeError("Variable \"" + identifier + "\" of type \"" + variable.GetType() + "\" can not accept value \"" + input + "\".").Report(ioContext);
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
                            else new RuntimeError("Variable \"" + identifier + "\" of type \"" + variable.GetType() + "\" can not accept value \"" + input + "\".").Report(ioContext);
                            return;
                        }
                }
            }
            else
            {
                new RuntimeError("Use of undeclared identifier \"" + read.identifier.name + "\".").Report(ioContext);  
            }
        }

        private void interpretEcho(EchoNode echo, Scope currentScope)
        {
            ExpressionNode expression = echo.expression;

            object resolution = Expressions.ResolveExpression(expression, currentScope, ioContext);

            if (resolution != null)
            {
                if (echo.isNewLine) ioContext.WriteLine(resolution.ToString());
                else ioContext.Write(resolution.ToString());
            }
        }

        private void interpretClear()
        {
            ioContext.Clear();
        }

        private void interpretConditional(ConditionNode condition, Scope currentScope)
        {
            bool resolution = Expressions.ResolveRelationalExpression(condition.expression, currentScope, ioContext);

            List<ParseNode> gotoTrue = condition.gotoTrue;

            if (resolution) Execute(condition.gotoTrue, currentScope);
        }

        private void interpretWhileLoop(WhileLoopNode loop, Scope currentScope)
        {
            bool resolution = Expressions.ResolveRelationalExpression(loop.expression, currentScope, ioContext);
            List<ParseNode> gotoLoop = loop.gotoLoop;

            while (resolution)
            {
                resolution = Expressions.ResolveRelationalExpression(loop.expression, currentScope, ioContext);
                if (resolution) Execute(gotoLoop, currentScope);

                if (AbortExecution) { break; }
            }
        }
    }
}
