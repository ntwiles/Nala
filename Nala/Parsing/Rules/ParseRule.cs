using System;
using System.Collections.Generic;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;

namespace NathanWiles.Nala.Parsing.Rules
{
    public abstract class ParseRule
    {
        // Does a quick check to detect if the user is trying to implement this language feature.
        public abstract bool Matches(List<NalaToken> sentence, IIOContext ioContext);

        // Does an indepth check to see if the implementation is syntactically correct.
        public abstract bool IsProper(List<NalaToken> sentence, IIOContext ioContext);
    }   
}
