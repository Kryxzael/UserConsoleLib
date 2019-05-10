using System;
using System.Collections.Generic;
using System.Linq;

namespace UserConsoleLib.StandardLib.Control
{
    class And : Command
    {
        public override string Name => "and";

        public override string HelpDescription => "Returns true if all values are true";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Booleans");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            List<bool> l = new List<bool>();

            for (int i = 0; i < args.Count; i++)
            {
                if (args.IsBoolean(i))
                {
                    l.Add(args.ToBoolean(i));
                }
                else
                {
                    target.WriteWarning("'" + args[i] + "' was not a boolean, ignored");
                }
            }

            if (!l.Any())
            {
                ThrowArgumentError(args[0], ErrorCode.NO_VALID_VALUES);
            }
            target.WriteLine(l.All(i => i));


        }
    }
}
