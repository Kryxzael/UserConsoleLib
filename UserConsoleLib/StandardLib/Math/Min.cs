using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Min : Command
    {
        public override string Name => "min";

        public override string HelpDescription => "Returns the smallest number of all arguments";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Numbers (#)");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            List<double> l = new List<double>();

            for (int i = 0; i < args.Count; i++)
            {
                if (args.IsDouble(i))
                {
                    l.Add(args.ToDouble(i));
                }
                else
                {
                    target.WriteWarning("'" + args[i] + "' was not a number, ignored");
                }
            }

            if (!l.Any())
            {
                ThrowNaNError(args[0], ErrorCode.NO_VALID_VALUES);
            }
            target.WriteLine(l.Min());
        }
    }
}
