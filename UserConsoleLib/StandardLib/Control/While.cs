using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    internal class While : Command
    {
        public override string Name
        {
            get
            {
                return "while";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Repeats a block of code until an expression is false";
            }
        }

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
            ScriptTargetWrapper stw = target as ScriptTargetWrapper;

            if (Boolean.Evaluate(args))
            {

                stw.Session.Scope.Push(new ScopeLoopPoint(ignoreLines: false, loopPoint: stw.Session.Index, target: stw));
            }
            else
            {
                stw.Session.Scope.Push(new ScopeLoopPoint(ignoreLines: true, target: stw));
            }
        }

        internal override bool IsCodeBlockCommand()
        {
            return true;
        }
    }
}
