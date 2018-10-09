using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    internal class DebugBreak : Command
    {
        public override string Name
        {
            get
            {
                return "breakpoint";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "If a debuger is running, the debuger will break when this line is executed";
            }
            
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if ((target is ScriptTargetWrapper))
            {
                (target as ScriptTargetWrapper).Session.ShouldBreakDebugger = true;
            }
            else
            {
                target.WriteError("Not in scripthost");
            }
        }
    }
}
