using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Variables
{
    internal class Varop : Command
    {
        public override string Name => "varop";
        public override string HelpDescription => "Modifies a variable";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Variable name").Or()
                .Add("Variable name").Add("", "=").AddTrailing("Value").Or()
                .Add("Variable name").Add("", "+=", "-=", "*=", "/=", "%=").Add("Value", Range.INFINITY, false).Or()
                .Add("Variable name").Add("", "++", "--").Or()
                .Add("?");


        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            VariableCollection vars = scope.Variables;

            if (args[0] == "?")
            {
                target.WriteLine("=   Sets the value of a variable");
                target.WriteLine("+=  Increments a variable by an amount");
                target.WriteLine("-=  Decrements a variable by an amount");
                target.WriteLine("*=  Multiplies a variable by an amount");
                target.WriteLine("/=  Divides a variable by an amount");
                target.WriteLine("++  Increments a variable by 1");
                target.WriteLine("--  Decrements a variable by 1");
                return;
            }

            if (!vars.IsDefined(args[0]))
            {
                ThrowGenericError("No such variable with name '" + args[0] + "' is defined'", ErrorCode.ARGUMENT_UNLISTED);
            }

            if (args.Count == 1)
            {
                target.WriteLine(vars.Get(args[0]));
            }
            else 
            {
                string set;

                switch (args[1])
                {
                    case "=":
                        set = args.JoinEnd(2);
                        break;
                    case "+=":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value + args.ToDouble(2));
                        break;
                    case "-=":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value - args.ToDouble(2));
                        break;
                    case "*=":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value * args.ToDouble(2));
                        break;
                    case "/=":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value / args.ToDouble(2));
                        break;
                    case "%=":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value % args.ToDouble(2));
                        break;
                    case "++":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value + 1);
                        break;
                    case "--":
                        set = ConConverter.ToString(ConConverter.ToDouble(vars.Get(args[0])).Value + 2);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                vars.Set(args[0], set);
            }
        }
    }
}
