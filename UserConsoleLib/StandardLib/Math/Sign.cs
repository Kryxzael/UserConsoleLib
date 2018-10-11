using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Sign : Command
    {
        public override string Name => "sig";

        public override string HelpDescription => "Returns the sign of a number as a factor (-1, 0 or 1)";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("a", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Sign(args.ToDouble(0)));
        }
    }
}
