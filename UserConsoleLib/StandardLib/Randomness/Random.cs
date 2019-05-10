using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Randomness
{
    class Random : Command
    {
        internal static System.Random rng = new System.Random();

        public override string Name => "random";

        public override string HelpDescription => "Returns a random number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Max", Range.From(1), true).Or().Add("Min", Range.INFINITY, true).Add("Max", Range.INFINITY, true).Or();
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            if (args.Count == 0)
            {
                target.WriteLine(rng.Next());
            }
            else if (args.Count == 1)
            {
                target.WriteLine(rng.Next(args.ToInt(0)));
            }
            else if (args.Count == 2)
            {
                if (args.ToInt(1) <= args.ToInt(0))
                {
                    ThrowOutOfRangeError(args.ToInt(1), Range.From(args.ToInt(0) + 1), ErrorCode.NUMBER_TOO_SMALL);
                }

                target.WriteLine(rng.Next(args.ToInt(0), args.ToInt(1)));
            }
        }
    }
}
