using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserConsoleLib.StandardLib
{
    /// <summary>
    /// System command that calls Application.Exit. Only works if a message pump is running
    /// </summary>
    internal class Exit : Command
    {
        public override string Name => "exit";

        public override string HelpDescription => "If the current application is a forms application: Closes the application";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteWarning("!!!SHUTTING DOWN!!!");
            Environment.Exit(0);
        }
    }
}
