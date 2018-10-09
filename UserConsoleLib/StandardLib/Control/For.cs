using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    internal class For : Command
    {
        public override string Name
        {
            get
            {
                return "for";
            }
        }

        public override string HelpDescription
        {
            get
            {
                return "Loops a block of code over every element in a collection";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Variable name").Add("", "in").AddTrailing("Array or list {");
        }

        internal override bool IsCodeBlockCommand()
        {
            return true;
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Last() != "{")
            {
                ThrowSyntaxError(this, args, ErrorCode.NOT_ENOUGH_ARGUMENTS);
            }

            ScriptTargetWrapper stw = target as ScriptTargetWrapper;

            //If we are currently in a loop
            if (EndBlock.LastPoppedEnumerator != null)
            {
                //If the loop is not finished, forward it
                if (EndBlock.LastPoppedEnumerator.MoveNext())
                {
                    stw.Session.Scope.Push(new ScopeLoopPoint(EndBlock.LastPoppedEnumerator, stw.Session.Index, stw));
                    stw.Session.Scope.Peek().Locals.Set(args[0], EndBlock.LastPoppedEnumerator.Current);
                }

                //If the loop is finished, ignore lines
                else
                {
                    stw.Session.Scope.Push(new ScopeLoopPoint(ignoreLines: true, target: stw));
                }

                EndBlock.LastPoppedEnumerator = null;
            }

            //If we are starting a loop
            else
            {
                IEnumerator<string> _;

                //If we are looping over a list
                if (args[2].StartsWith("["))
                {
                    _ = ConConverter.GetList(args[2]).GetEnumerator();
                }

                //If we are looping over an array
                else
                {                       //take to prevent getting the {
                    _ = args.Skip(2).Take(args.Count - 3).GetEnumerator();
                }

                //Push the enumerator (yes we take the recursion way, shouldn't be a problem)
                EndBlock.LastPoppedEnumerator = _;
                Executed(args, target);
                
            }
        }
    }
}
