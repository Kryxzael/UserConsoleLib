using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib
{
    /// <summary>
    /// System command that displays help and syntax of/about command, or lists all commands
    /// </summary>
    internal class Help : Command
    {
        public override string Name => "help";

        public override string HelpDescription => "Lists commands or displays detailed information about them";

        public override Syntax GetSyntax(Params args)
        {
            if (args.Count == 0)
            {
                return Syntax.Begin().Add("Command name", AllCommandsInternal.Select(i => i.Name).ToArray()).Or().Add("Page number", Range.From(1).To(GetPageCount()), true).Or();
            }
            else if (args.Count == 1 && args.IsDouble(0))
            {
                return Syntax.Begin().Add("Page number", Range.From(1).To(GetPageCount()), true);
            }
            else
            {
                return Syntax.Begin().Add("Command name", AllCommandsInternal.Select(i => i.Name).ToArray());
            }
            
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            Command[] enabledCommands = AllCommandsInternal.Where(i => i.IsEnabled).ToArray();

            if (args.Count == 0)
            {
                Executed(new Params(new string[] { 1.ToString() }), target, scope);
                return;
            }

            if (args.IsInteger(0))
            {
                target.WriteLine("Listing commands at page " + args.ToInt(0) + " of " + GetPageCount());

                for (int i = (args.ToInt(0) - 1) * 5; i < (args.ToInt(0) - 1) * 5 + 5 && i < enabledCommands.Length; i++)
                {
                    target.WriteLine("* " + enabledCommands[i].Name + ": " + enabledCommands[i].HelpDescription);
                }
            }
            else
            {
                Command cmd = GetByName(args.JoinEnd(0));

                target.WriteLine(cmd.Name);
                target.WriteLine("* " + cmd.HelpDescription);

                string sstring = cmd.GetSyntax(new Params(new string[0])).CreateSyntaxString();
                target.WriteWarning("* Syntax: ");

                foreach (string i in sstring.Split('\n'))
                {
                    target.WriteWarning("*   " + i);
                }

            }
        }

        static int GetPageCount()
        {
            return AllCommandsInternal.Count(i => i.IsEnabled) / 5 + (AllCommandsInternal.Count(i => i.IsEnabled) % 5 == 0 ? 0 : 1);
        }
    }
}
