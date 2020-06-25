using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NalaWeb.Models
{
    public class NalaResult
    {
        public bool LexingSuccessful { get; set; } = false;
        public bool ParsingSuccessful { get; set; } = false;

        public List<string> Output { get; set; }
    }
}
