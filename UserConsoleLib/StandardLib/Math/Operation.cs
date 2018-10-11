using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class Operation : Command
    {
        public override string Name => "op";

        public override string HelpDescription => "Evaluates a mathematical expression";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Operand", Range.INFINITY, false).Add("Operator", "+", "-", "*", "/", "%").Add("Operand", Range.INFINITY, false);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            switch (args[1])
            {
                case "+":
                    target.WriteLine(args.ToDouble(0) + args.ToDouble(2));
                    break;
                case "-":
                    target.WriteLine(args.ToDouble(0) - args.ToDouble(2));
                    break;
                case "*":
                    target.WriteLine(args.ToDouble(0) * args.ToDouble(2));
                    break;
                case "/":
                    if (args.ToInt(2) == 0)
                    {
                        ThrowGenericError("Attempted division by zero", ErrorCode.NUMBER_IS_ZERO);
                    }

                    target.WriteLine(args.ToDouble(0) / args.ToDouble(2));
                    break;
                case "%":
                    target.WriteLine(args.ToDouble(0) % args.ToDouble(2));
                    break;
                default:
                    ThrowArgumentError(args[1], new string[] { "+", "-", "*", "/" }, ErrorCode.ARGUMENT_UNLISTED);
                    break;
            }

        }
    }
}
