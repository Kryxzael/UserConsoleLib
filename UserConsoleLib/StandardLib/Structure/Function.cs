using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Structure
{
    internal class Function : Command
    {
        public override string Name
        {
            get
            {
                return "func";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Begins a function definition";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Identifier").AddTrailing("Params");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            (target as ScriptTargetWrapper).Session.Scope.Push(new ScopeLoopPoint(ignoreLines: true, target: target as ScriptTargetWrapper));
            Register(new Call(new FunctionInfo(args[0], (target as ScriptTargetWrapper).Session.Index)));
        }

        internal override bool IsCodeBlockCommand()
        {
            return true;
        }
    }
}
