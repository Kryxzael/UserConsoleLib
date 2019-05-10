using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Variables
{
    class Var : Command
    {
        public override string Name => "var";
        public override string HelpDescription => "Fefines a variable in the immediate scope";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Variable name").Or()
                .Add("Variable name").Add("", "=").AddTrailing("Value");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            VariableCollection vars = scope.Variables;

            //Throw if a variable with the given name already exists in the immediate scope
            if (vars.AllVariables.Any(i => i.Key == args[0]))
            {
                throw new CommandException("A variable with that name if already defined in the immediate scope", ErrorCode.NUMBER_IS_POSITIVE);
            }

            if (args.Count == 1)
            {
                vars.Set(args[0], "");
            }
            else
            {
                vars.Set(args[0], args.JoinEnd(2));
            }
        }
    }
}
