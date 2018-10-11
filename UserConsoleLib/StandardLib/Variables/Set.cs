using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Variables
{
    class Set : Command
    {
        public override string Name => "set";

        public override string HelpDescription => "Assigns a value to a variable";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Name").Add("Operation", "=").AddTrailing("Value").Or()
                .Add("Name").Add("Operation", "+=", "-=", "*=", "/=").Add("Value", Range.INFINITY, false).Or()
                .Add("Name").Add("Operation", "++", "--").Or()
                .Add("Help", "?");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
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

            VariableCollection var;

            if (target is ScriptTargetWrapper o)
            {
                var = o.Session.Scope.Peek().Locals;
            }
            else
            {
                var = VariableCollection.GlobalVariables;
            }

            switch (args[1])
            {
                case "=":
                    var.Set(args[0], args.JoinEnd(2));
                    break;
                case "+=":
                    double v;
                    double.TryParse(var.Get(args[0]), out v);
                    var.Set(args[0], v + args.ToDouble(2));
                    break;
                case "-=":
                    double u;
                    double.TryParse(var.Get(args[0]), out u);
                    var.Set(args[0], u - args.ToDouble(2));
                    break;
                case "*=":
                    double w;
                    double.TryParse(var.Get(args[0]), out w);
                    var.Set(args[0], w * args.ToDouble(2));
                    break;
                case "/=":
                    double x;
                    double.TryParse(var.Get(args[0]), out x);
                    var.Set(args[0], x + args.ToDouble(2));
                    break;
                case "++":
                    double y;
                    double.TryParse(var.Get(args[0]), out y);
                    var.Set(args[0], y + 1);
                    break;
                case "--":
                    double z;
                    double.TryParse(var.Get(args[0]), out z);
                    var.Set(args[0], z - 1);
                    break;
            }
        }
    }
}
