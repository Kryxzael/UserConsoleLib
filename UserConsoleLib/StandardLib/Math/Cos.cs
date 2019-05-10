using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Cos : Command
    {
        public override string Name => "cos";

        public override string HelpDescription => "Returns the cosine of a number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("a", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(System.Math.Cos(args.ToDouble(0)));
        }
    }
}
