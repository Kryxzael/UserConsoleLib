using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib
{
    /// <summary>
    /// System command that prints a line of text to the target IConsoleOutput
    /// </summary>
    internal class Echo : Command
    {
        public override string Name => "echo";

        public override string HelpDescription => "Writes a line of text to the console";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Text");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(args.JoinEnd(0));
        }
    }
}
