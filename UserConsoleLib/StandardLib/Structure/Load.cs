using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Structure
{
    internal class Load : Command
    {
        public override string Name => "load";
        public override string HelpDescription => "Loads a script file";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Path");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            try
            {
                Scope.FromLines(System.IO.File.ReadAllText(args.JoinEnd(0)), scope).RunScope(target);
            }
            catch (Exception ex)
            {
                ThrowGenericError("Could not open file for read: " + ex.Message, ErrorCode.IO_ERROR);
            }
        }
    }
}
