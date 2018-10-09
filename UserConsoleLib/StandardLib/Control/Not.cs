using System;
using System.Collections.Generic;
using System.Linq;

namespace UserConsoleLib.StandardLib.Control
{
    class Not : Command
    {
        public override string Name => "not";

        public override string HelpDescription => "Negates a boolean value";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Boolean", true);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(!args.ToBoolean(0));
        }
    }
}
