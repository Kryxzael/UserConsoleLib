using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    internal class EndBlock : Command
    {
        public static IEnumerator<string> LastPoppedEnumerator;

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
                if ((target as ScriptTargetWrapper).Session.Scope.Any())
                {
                    ScopeLoopPoint item = (target as ScriptTargetWrapper).Session.Scope.Pop();

                    if (item.HasReturnPoint)
                    {
                        (target as ScriptTargetWrapper).Session.Index = item.ReturnPoint - 1; //TODO double check
                    }
                    if (item.Enumerator != null)
                    {
                        LastPoppedEnumerator = item.Enumerator;
                    }
                    return;
                }

                ThrowGenericError("Stack underflow", ErrorCode.NUMBER_TOO_SMALL);
            }
        }

        internal override bool IsCodeBlockCommand()
        {
            return true;
        }
    }
}
