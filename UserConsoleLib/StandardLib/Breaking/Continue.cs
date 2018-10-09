using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Breaking
{
    internal class Continue : Command
    {
        public override string Name
        {
            get
            {
                return "continue";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Terminates the current loop instance and starts the next one";
            }
        }

        protected bool Break { get; set; }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper i)
            {
                if (i.Session.Scope.Any(o => o.HasReturnPoint))
                {
                    while (true)
                    {
                        ScopeLoopPoint loopPoint = i.Session.Scope.Pop();

                        if (loopPoint.HasReturnPoint)
                        {
                            i.Session.Index = loopPoint.ReturnPoint - (Break ? 0 : 1);

                            //Makes sure to 'break' not 'continue'
                            if (Break)
                            {
                                i.Session.Scope.Push(new ScopeLoopPoint(ignoreLines: true, target: i));
                            }
                            break;
                        }
                    }
                }
                else
                {
                    ThrowGenericError("Cannot use break! Not in loop or function", ErrorCode.INVALID_CONTEXT);
                }
            }
            else
            {
                ThrowGenericError("This command is only valid in script files", ErrorCode.INVALID_CONTEXT);
            }
        }
    }
}
