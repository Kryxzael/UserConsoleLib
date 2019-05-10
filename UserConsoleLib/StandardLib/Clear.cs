using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib
{
    /// <summary>
    /// System command that clears calls IConsoleOutput.ClearBuffer()
    /// </summary>
    internal class Clear : Command
    {
        public override string Name => "clear";

        public override string HelpDescription => "Clears the screen";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.ClearBuffer();
        }
    }
}
