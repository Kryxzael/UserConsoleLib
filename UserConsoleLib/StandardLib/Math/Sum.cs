using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Sum : Command
    {
        public override string Name => "sum";

        public override string HelpDescription => "Returns the sum of a set of numbers";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Numbers (#)");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
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

            target.WriteLine(l.Sum());
        }
    }
}
