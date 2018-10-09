using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Brackets
{
    class BracketOpen : Command
    {
        public override string Name
        {
            get
            {
                return "bo";
            }
        }

        public override string HelpDescription
        {
            get
            {
                return "Escapes the ( symbol";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine("(");
        }
    }

    class BracketClose : Command
    {
        public override string Name
        {
            get
            {
                return "bc";
            }
        }

        public override string HelpDescription
        {
            get
            {
                return "Escapes the ) symbol";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(")");
        }

      
    }

    class Brac : Command
    {
        public override string Name
        {
            get
            {
                return "brac";
            }
        }

        public override string HelpDescription
        {
            get
            {
                return "Encapsulates the given string in brackets";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Or()
                .AddTrailing("text");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count != 0)
            {
                target.WriteLine("(" + args.JoinEnd(0) + ")");
            }
            else
            {
                target.WriteLine("()");
            }

        }
    }
}
