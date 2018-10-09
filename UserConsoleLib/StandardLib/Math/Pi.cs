using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Pi : Command
    {
        public override string Name => "pi";

        public override string HelpDescription => "Returns the value of the mathematical constant π";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.PI);
        }
    }
}
