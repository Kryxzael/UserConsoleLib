using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    internal class Floor : Command
    {
        public override string Name
        {
            get
            {
                return "floor";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Rounds a number downwards to the closest integer";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Value", float.NegativeInfinity, float.PositiveInfinity, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(System.Math.Floor(args.ToDouble(0)));
        }
    }
}
