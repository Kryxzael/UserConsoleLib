using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    internal class Sqrt : Command
    {
        public override string Name
        {
            get
            {
                return "sqrt";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Returns the square root of a number";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Value", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Sqrt(args.ToDouble(0)));
        }
    }
}
