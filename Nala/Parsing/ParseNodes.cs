
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing
{    
    public class ParseNode
    {
        public ParseNode Parent;
    }

    //TODO: Move these out into their own files.

    public class LiteralNode : ParseNode { }

    public class IntNode : LiteralNode
    { 
        public int value; 
    
        public IntNode(int value) { this.value = value; }
        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class StringNode : LiteralNode
    {
        public string value;

        public StringNode(string value) { this.value = value; }

        public override string ToString()
        {
            return value;
        }
    }

    public class BoolNode : LiteralNode
    {
        public bool value;

        public BoolNode(bool value) { this.value = value; }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class ReadNode : ParseNode
    {
        public IdentifierNode identifier;
    }

    public class EchoNode : ParseNode
    {
        public ExpressionNode expression;
        public bool isNewLine = false;
    }

    public class ClearNode : ParseNode { }

    public class WhileLoopNode : ParseNode
    {
        public ExpressionNode expression;
        public List<ParseNode> gotoLoop;
    }

    public class ConditionNode : ParseNode
    {
        public ExpressionNode expression;
        public List<ParseNode> gotoTrue;
    }

    public class FunctionNode : ParseNode
    {
        public List<ParseNode> goToInvoke;
    }

    public class ExpressionNode : ParseNode
    {
        public List<ParseNode> elements;
        public bool isRelational;

        public ExpressionNode()
        {
            elements = new List<ParseNode>();
        }
    }

    public class IdentifierNode : ParseNode
    {
        public string name;

        public bool hasIndexer;
        public ParseNode index;

        public IdentifierNode(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class OperatorNode : ParseNode
    {
        public string symbol;
        public OperatorType type;

        public OperatorNode(string symbol)
        {
            this.symbol = symbol;

            if (TokenLookups.ArithOperators.Contains(symbol)) { type = OperatorType.Arithmatic; }
            else if (TokenLookups.RelationOperators.Contains(symbol)) { type = OperatorType.Relational; }
        }

        public override string ToString()
        {
            return symbol;
        }
    }
}
