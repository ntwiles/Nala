
using System;
using System.Collections.Generic;

namespace NathanWiles.Nala.Lexing
{
    public static class TokenLookups
    {
        public static List<string> Operators,
            ArithOperators, AssignOperators, RelationOperators,
            Punctuators, Primitives, Keywords;

        static TokenLookups()
        {
            Operators = new List<string>
            {
                "!","++", "--",
            };

            RelationOperators = new List<string>
            {
                "==", "!=",">","<",
                ">=","<="
            };

            ArithOperators = new List<string>
            {
                "+","-","*","/","%"
            };

            AssignOperators = new List<string>
            {
                "=","+=","-=","*=","/="
            };

            Punctuators = new List<string>
            {
                "(",")",
                "{","}",
                "[","]",
                ";",
                ","
            };

            Primitives = new List<string>
            {
                "int","string","bool"
            };

            Keywords = new List<string>
            {
                "echo","echoline","clear",
                "wiles", "if", "read"
            };

            Operators.AddRange(ArithOperators);
            Operators.AddRange(AssignOperators);
            Operators.AddRange(RelationOperators);
        }
    }
}
