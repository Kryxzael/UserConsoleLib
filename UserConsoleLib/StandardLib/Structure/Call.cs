using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Structure
{
    internal class Call : Command
    {
        FunctionInfo _function;
        public override string Name
        {
            get
            {
                return _function.Name;
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "null";
            }
        }

        public Call(FunctionInfo functionInfo)
        {
            _function = functionInfo;
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or().AddTrailing("Arguments");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper i)
            {
                i.Session.Scope.Push(new ScopeLoopPoint(false, i.Session.Index + 1, target: i));
                i.Session.Index = _function.Line;
            }
            else
            {
                ThrowGenericError("Cannot fire function from console interface. This is an internal error", ErrorCode.INTERNAL_ERROR);
            }
        }
    }
}
