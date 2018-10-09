using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class Label : Command
    {
        public override string Name => "label";

        public override string HelpDescription => "Defines a label in a script file that can be jumped to using 'skip'";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or().Add("Label name");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (!(target is ScriptTargetWrapper))
            {
                ThrowGenericError("This command is invalid in console mode", ErrorCode.INVALID_CONTEXT);
            }
        }
    }
}
