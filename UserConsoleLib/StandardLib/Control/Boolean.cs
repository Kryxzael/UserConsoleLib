using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class Boolean : Command
    {
        public override string Name => "bool";

        public override string HelpDescription => "Evaluates a logical expression";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Operand 1").Add("Operator", "==", "!=").Add("Operand 2").Or()
                .Add("Operand 1", Range.INFINITY, false).Add("Operator", ">", "<", ">=", "<=").Add("Operand 2", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(Evaluate(args));
        }

        //BM compare evaluation
        public static bool Evaluate(Params args)
        {
            if (args[0] == "not")
            {
                return !args.ToBoolean(1);
            }
            else
            {
                switch (args[1])
                {
                    case "==":
                        return args[0] == args[2];
                    case "!=":
                        return args[0] != args[2];
                    case ">":
                        return args.ToDouble(0) > args.ToDouble(2);
                    case "<":
                        return args.ToDouble(0) < args.ToDouble(2);
                    case ">=":
                        return args.ToDouble(0) >= args.ToDouble(2);
                    case "<=":
                        return args.ToDouble(0) <= args.ToDouble(2);
                    default:
                        if (args.IsBoolean(0))
                        {
                            return args.ToBoolean(0);
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                }
            }
        }
    }
}
