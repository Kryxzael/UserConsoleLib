using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Variables
{
    class Range : Command
    {
        public override string Name => "range";

        public override string HelpDescription => "Creates a list containing a set amount of integers starting at a specific number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Count", 1, short.MaxValue, true).Or().Add("Start", int.MinValue, int.MaxValue, true).Add("Count", 1, short.MaxValue, true);
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count == 1)
            {
                target.WriteLine(string.Join(" ", Enumerable.Range(0, args.ToInt(0)).Select(i => ConConverter.ToString(i)).ToArray()));
            }
            else
            {
                target.WriteLine(string.Join(" ", Enumerable.Range(args.ToInt(0), args.ToInt(1)).Select(i => ConConverter.ToString(i)).ToArray()));
            }
            
        }
    }
}
