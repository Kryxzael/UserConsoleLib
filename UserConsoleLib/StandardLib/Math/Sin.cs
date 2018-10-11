using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Sin : Command
    {
        public override string Name => "sin";

        public override string HelpDescription => "Returns the sine of a number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("a", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Sin(args.ToDouble(0)));
        }
    }
}
