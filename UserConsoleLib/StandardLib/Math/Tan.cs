using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Tan : Command
    {
        public override string Name => "tan";

        public override string HelpDescription => "Returns the tangent of an angle";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("a", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(System.Math.Tan(args.ToDouble(0)));
        }
    }
}
