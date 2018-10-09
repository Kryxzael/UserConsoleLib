using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Variables
{
    internal class String : Command
    {
        public override string Name
        {
            get
            {
                return "str";
            }
        }
        public override string HelpDescription
        {
            get
            {
                return "Allows modifications of strings";
            }
        }

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Add("Operation", "toarray", "tolist", "length", "tolower", "toupper").AddTrailing("Input").Or()
                .Add("Operation", "split", "startswith", "endswith", "contains", "replacespace").Add("Inner value").AddTrailing("Input").Or()
                .Add("Operation", "replace").Add("Replace").Add("Replace with").AddTrailing("Input").Or()
                .Add("Operation", "skip").Add("Start index", 0, int.MaxValue, true).AddTrailing("Input").Or()
                .Add("Operation", "substring").Add("Start index", 0, int.MaxValue, true).Add("Length", 0, int.MaxValue, true).AddTrailing("Input");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            switch (args[0])
            {
                case "toarray":
                    target.WriteLine(string.Join(" ", args.JoinEnd(1).ToCharArray().Select(i => i.ToString()).ToArray()));
                    break;
                case "tolist":
                    throw new NotImplementedException(); //TODO NYI List
                case "length":
                    target.WriteLine(args.JoinEnd(1).Length);
                    break;
                case "tolower":
                    target.WriteLine(args.JoinEnd(1).ToLower());
                    break;
                case "toupper":
                    target.WriteLine(args.JoinEnd(1).ToUpper());
                    break;
                case "split":
                    target.WriteLine(string.Join(" ", args.JoinEnd(2).Split(args[1].Single())));
                    break;
                case "startswith":
                    target.WriteLine(args.JoinEnd(2).StartsWith(args[1]));
                    break;
                case "endswith":
                    target.WriteLine(args.JoinEnd(2).EndsWith(args[1]));
                    break;
                case "contains":
                    target.WriteLine(args.JoinEnd(2).Contains(args[1]));
                    break;
                case "replacespace":
                    target.WriteLine(args.JoinEnd(2).Replace(" ", args[1]));
                    break;
                case "replace":
                    target.WriteLine(args.JoinEnd(3).Replace(args[1], args[2]));
                    break;
                case "skip":
                    int skipIndex = args.ToInt(1);
                    string input = args.JoinEnd(2);

                    if (skipIndex >= input.Length)
                    {
                        ThrowOutOfRangeError(skipIndex, 0, input.Length - 1, ErrorCode.NUMBER_OUT_OF_RANGE);
                    }

                    target.WriteLine(input.Substring(skipIndex));
                    break;
                case "substring":
                    int substrStart = args.ToInt(1);
                    int substrLength = args.ToInt(2);
                    string substrInput = args.JoinEnd(3);

                    if (substrStart >= substrInput.Length)
                    {
                        ThrowOutOfRangeError(substrStart, 0, substrInput.Length - 1, ErrorCode.NUMBER_OUT_OF_RANGE);
                    }

                    if (substrLength + substrStart > substrInput.Length)
                    {
                        ThrowOutOfRangeError(substrLength, 0, substrInput.Length - substrStart, ErrorCode.NUMBER_OUT_OF_RANGE);
                    }

                    target.WriteLine(substrInput.Substring(substrStart, substrLength));
                    
                    break;
            }
        }
    }
}
