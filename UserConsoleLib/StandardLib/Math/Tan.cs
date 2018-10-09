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
            return Syntax.Begin().Add("a", double.NegativeInfinity, double.PositiveInfinity, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Tan(args.ToDouble(0)));
        }
    }
}
