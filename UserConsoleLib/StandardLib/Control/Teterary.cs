using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class Teterary : Command
    {
        public override string Name
        {
            get
            {
                return "iftrue";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Returns the first argument if the expression is true";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            throw new NotImplementedException();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            throw new NotImplementedException();
        }
    }
}
