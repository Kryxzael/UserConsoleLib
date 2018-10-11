using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Variables
{
    class RangeCommand : Command
    {
        public override string Name => "range";

        public override string HelpDescription => "Creates a list containing a set amount of integers starting at a specific number";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Count", Range.From(1), true).Or().Add("Start", Range.INFINITY, true).Add("Count", Range.From(1), true);
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
