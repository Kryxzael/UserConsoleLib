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
            return Syntax.Begin().Add("Max", 1, int.MaxValue, true).Or().Add("Min", int.MinValue, int.MaxValue, true).Add("Max", int.MinValue, int.MaxValue, true).Or();
        }

        protected override void Executed(Params args, IConsoleOutput target)
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
                    ThrowOutOfRangeError(args.ToInt(1), args.ToInt(0) + 1, int.MaxValue, ErrorCode.NUMBER_TOO_SMALL);
                }

                target.WriteLine(rng.Next(args.ToInt(0), args.ToInt(1)));
            }
        }
    }
}
