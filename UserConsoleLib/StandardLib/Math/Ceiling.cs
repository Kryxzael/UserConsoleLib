using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    internal class Ceiling : Command
    {
        public override string Name
        {
            get
            {
                return "ceil";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Rounds a number upwards to the closest integer";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Value", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Ceiling(args.ToDouble(0)));
        }
    }
}
