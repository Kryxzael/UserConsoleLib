using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.ExtendedLib.DateTime
{
    class Time : Command
    {
        public override string Name
        {
            get
            {
                return "time";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Retrieves the current time";
            }
        }        

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .AddTrailing("format");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count == 0)
            {
                target.WriteLine(System.DateTime.Now.ToShortTimeString());
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
