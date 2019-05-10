using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    internal class Round : Command
    {
        public override string Name
        {
            get
            {
                return "round";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Rounds a number to the closest integer";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Value", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(System.Math.Round(args.ToDouble(0)));
        }
    }
}
