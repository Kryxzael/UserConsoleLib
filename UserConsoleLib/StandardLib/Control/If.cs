using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class If : Command
    {
        public override string Name => "if";

        public override string HelpDescription => "Invokes a command if an expression is true";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Expression", true).Add("", "{").Or()
                .Add("not", "not").Add("Expression", true).Add("", "{").Or()
                .Add("Operand 1").Add("Operator", "==", "!=").Add("Operand 2").Add("", "{").Or()
                .Add("Operand 1", double.NegativeInfinity, double.PositiveInfinity, false).Add("Operator", ">", "<", ">=", "<=").Add("Operand 2", double.NegativeInfinity, double.PositiveInfinity, false).Add("", "{");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            (target as ScriptTargetWrapper).Session.Scope.Push(new ScopeLoopPoint(ignoreLines: !Boolean.Evaluate(args), target: target as ScriptTargetWrapper));
        }

        internal override bool IsCodeBlockCommand()
        {
            return true;
        }
    }
}
