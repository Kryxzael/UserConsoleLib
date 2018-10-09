using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    internal class EndBlock : Command
    {        
        public override string Name
        {
            get
            {
                return "}";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Ends an code block";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper)
            {
                if ((target as ScriptTargetWrapper).Session.ScopeDepth.Any())
                {
                    ScopeLoopPoint item = (target as ScriptTargetWrapper).Session.ScopeDepth.Pop();

                    if (item.HasLoopPoint)
                    {
                        (target as ScriptTargetWrapper).Session.Index = item.LoopPoint - 1; //TODO double check
                    }
                    return;
                }

                ThrowGenericError("Stack underflow", ErrorCode.NUMBER_TOO_SMALL);
            }
        }

        public override bool IsCodeBlockCommand()
        {
            return true;
        }
    }
}
