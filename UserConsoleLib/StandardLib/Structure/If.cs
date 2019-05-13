using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Structure
{
    internal class If : Command
    {
        public override string Name => "if";
        public override string HelpDescription => "Executes a code block if the given expression is true";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Expression", false).AddTrailing("{}");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            if (args.ToBoolean(0))
            {
                args.ToScope(1, scope).RunScope(target);
            }
        }
    }
}
