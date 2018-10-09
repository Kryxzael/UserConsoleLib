using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Breaking
{
    internal class Break : Continue
    {
        public Break()
        {
            Break = true;
        }

        public override string Name
        {
            get
            {
                return "break";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Breaks out of a loop";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin();
        }
    }
}
