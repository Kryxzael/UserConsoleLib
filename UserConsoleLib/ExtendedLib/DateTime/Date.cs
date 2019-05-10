using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.ExtendedLib.DateTime
{
    class Date : Command
    {
        public override string Name
        {
            get
            {
                return "date";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Retrieves the current date";
            }
        }        

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .AddTrailing("format");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            if (args.Count == 0)
            {
                target.WriteLine(System.DateTime.Now.ToShortDateString());
            }
            else
            {
                try
                {
                    target.WriteLine(System.DateTime.Now.ToString(args.JoinEnd(0)));
                }
                catch (FormatException)
                {
                    ThrowArgumentError(args.JoinEnd(1), ErrorCode.ARGUMENT_INVALID);
                }
            }
        }
    }
}
