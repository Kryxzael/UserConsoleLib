using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Breaking
{
    internal class Endscript : Command
    {
        public override string Name
        {
            get
            {
                return "endscript";
            }
        }

        public override string HelpDescription
        {
            get
            {
                return "Terminates script execution";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper i)
            {
                i.Session.Abort();
            }
            else
            {
                ThrowGenericError("This command is only valid in script files", ErrorCode.INVALID_CONTEXT);
            }
        }
    }
}
