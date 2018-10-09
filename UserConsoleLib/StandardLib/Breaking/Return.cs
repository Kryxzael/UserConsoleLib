using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Breaking
{
    internal class Return : Command
    {
        public override string Name
        {
            get
            {
                return "return";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Terminates a function and returns a value";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .Add("Return value");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            
        }
    }
}
