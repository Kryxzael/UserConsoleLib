using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Math
{
    class E : Command
    {
        public override string Name => "e";

        public override string HelpDescription => "Returns the value of the mathematical constant e";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            target.WriteLine(System.Math.E);
        }
    }
}
