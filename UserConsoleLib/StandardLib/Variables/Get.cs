using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Variables
{
    class Get : Command
    {
        public override string Name => "get";

        public override string HelpDescription => "Gets the raw value of a variable";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Variable name");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (target is ScriptTargetWrapper i)
            {
                target.WriteLine(i.Session.Scope.Peek().Locals.Get(args[0]));
            }
            else
            {
                target.WriteLine(VariableCollection.GlobalVariables.Get(args[0]));
            }
            
        }
    }
}
