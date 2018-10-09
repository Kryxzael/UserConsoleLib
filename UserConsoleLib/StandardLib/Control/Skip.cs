using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class Skip : Command
    {
        public override string Name => "skip";

        public override string HelpDescription => "Jumps execution to a label in a script file";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or().Add("Count", 1, int.MaxValue, true).Or().Add("Name");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper)
            {
                if (args.Count == 0)
                {
                    (target as ScriptTargetWrapper).Session.SendSkipMessage(null, 1);
                }
                else if (args.IsInteger(0))
                {
                    (target as ScriptTargetWrapper).Session.SendSkipMessage(null, args.ToInt(0));
                }
                else
                {
                    (target as ScriptTargetWrapper).Session.SendSkipMessage(args[0], 0);
                }


            }
            else
            {
                ThrowGenericError("This command is invalid in console mode", ErrorCode.INVALID_CONTEXT);
            }

        }
    }
}
