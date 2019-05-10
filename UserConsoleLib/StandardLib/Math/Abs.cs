using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Abs : Command
    {
        public override string Name => "abs";

        public override string HelpDescription => "Returns the absolute value of a number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("n", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(System.Math.Abs(args.ToDouble(0)));
        }
    }
}
