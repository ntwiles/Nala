
using System;
using System.Collections.Generic;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Errors;
using NathanWiles.Nala.Parsing.NodeBuilders;
using NathanWiles.Nala.Parsing.Rules;

namespace NathanWiles.Nala.Parsing
{
    public class Parser
    {
        public bool AbortParse;

        private List<ParseBehavior> parseBehaviors;

        public Parser()
        {
            parseBehaviors = new List<ParseBehavior>();

            var bvrCondition = new ParseBehavior();
            bvrCondition.Rule = new ConditionParseRule();
            bvrCondition.Builder = new ConditionNodeBuilder();
            parseBehaviors.Add(bvrCondition);

            var bvrWhileLoop = new ParseBehavior();
            bvrWhileLoop.Rule = new WhileLoopParseRule();
            bvrWhileLoop.Builder = new WhileLoopNodeBuilder();
            parseBehaviors.Add(bvrWhileLoop);

            var bvrFunction = new ParseBehavior();
            bvrFunction.Rule = new FunctionParseRule();
            bvrFunction.Builder = new FunctionNodeBuilder();
            parseBehaviors.Add(bvrFunction);

            var bvrDeclaration = new ParseBehavior();
            bvrDeclaration.Rule = new DeclareParseRule();
            bvrDeclaration.Builder = new DeclarationNodeBuilder();
            parseBehaviors.Add(bvrDeclaration);

            var bvrAssignment = new ParseBehavior();
            bvrAssignment.Rule = new AssignParseRule();
            bvrAssignment.Builder = new AssignNodeBuilder();
            parseBehaviors.Add(bvrAssignment);

            var bvrEcho = new ParseBehavior();
            bvrEcho.Rule = new EchoParseRule();
            bvrEcho.Builder = new EchoNodeBuilder();
            parseBehaviors.Add(bvrEcho);

            var bvrRead = new ParseBehavior();
            bvrRead.Rule = new ReadParseRule();
            bvrRead.Builder = new ReadNodeBuilder();
            parseBehaviors.Add(bvrRead);

            var bvrClear = new ParseBehavior();
            bvrClear.Rule = new ClearParseRule();
            bvrClear.Builder = new ClearNodeBuilder();
            parseBehaviors.Add(bvrClear);
        }

        public List<ParseNode> ProcessTokens(List<NalaToken> tokens)
        {
            if (tokens.Count < 1) return null;

            List<ParseNode> parseTree = new List<ParseNode>();

            bool parsing = true;
            int position = 0;

            while (parsing)
            {
                // Get the next sentence.
                var sentence = GetNextSentence(position, tokens);

                position += sentence.Count;

                if (position >= tokens.Count - 1) parsing = false;

                // Determine what type of sentence it is.
                ParseNode node = getSentenceNode(sentence);

                if (node == null && !AbortParse)
                {
                    new ParseError(sentence[0], "Unknown sentence.").Report();
                    AbortParse = true;
                }

                if (AbortParse) { return null; }

                parseTree.Add(node);
            }
          
            return parseTree;
        }

        // TODO: This function seems to do two things; one) break a string of nodes into "sentences" terminated by ";"
        // or two) return a series of such sentences if they are nested inside {}s.
        // This confuses what it means to be a "sentence". Abstract out some of this into a different function, or rename this.
        public List<NalaToken> GetNextSentence(int position, List<NalaToken> tokens, bool nested = false)
        {
            List<NalaToken> sentence = new List<NalaToken>();

            bool building = true;

            while (building)
            {
                var token = tokens[position];

                sentence.Add(token);
                position++;

                if (token.value == "{")
                {
                    var block = GetNextSentence(position, tokens, true);
                    sentence.AddRange(block);
                    position += block.Count;

                    if (!nested) { return sentence; }
                }

                if (token.value == "}")
                {
                    return sentence;
                }

                if (token.value == ";" && !nested)
                {
                    return sentence;
                }

                if (position >= tokens.Count) return sentence;
            }

            return sentence;
        }

        private ParseNode getSentenceNode(List<NalaToken> sentence)
        {
            foreach (ParseBehavior behavior in parseBehaviors)
            {
                if (behavior.Rule.Matches(sentence))
                {
                    return behavior.Builder.BuildNode(sentence);
                }
            }

            return null;
        }
    }

    public enum OperatorType
    {
        Arithmatic,
        Relational
    }

    public enum PrimitiveType
    {
        Int,
        String,
        Bool
    }
}
