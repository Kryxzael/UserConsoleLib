using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Randomness
{
    class Choose : Command
    {
        public override string Name => "choose";

        public override string HelpDescription => "Chooses an argument passed at random";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Values");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            target.WriteLine(args[Random.rng.Next(args.Count)]);
        }
    }
}
