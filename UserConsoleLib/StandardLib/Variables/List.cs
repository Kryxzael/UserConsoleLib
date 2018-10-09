using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.StandardLib.Variables
{
    class List : Command
    {
        public override string Name => "list";

        public override string HelpDescription => "Creates and modifies lists";


        internal static Dictionary<int, List<string>> Registry = new Dictionary<int, List<string>>();
        static Random rng = new Random();

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("create").Or()
                .Add("create").AddTrailing("items").Or()
                .Add("pointer").Add("operation", "add", "remove", "contains").AddTrailing("item").Or()
                .Add("pointer").Add("operation", "addrange").AddTrailing("items").Or()
                .Add("pointer").Add("operation", "insert").Add("index", int.MinValue, int.MaxValue, true).AddTrailing("item").Or()
                .Add("pointer").Add("operation", "insertrange").Add("startindex", int.MinValue, int.MaxValue, true).AddTrailing("items").Or()
                .Add("pointer").Add("operation", "sort").Add("sortmode", "a", "z", "1").Or()
                .Add("pointer").Add("operation", "count", "flush", "clear", "reverse", "toarray").Or()
                .Add("pointer").Add("index", int.MinValue, int.MaxValue, true).Or()
                .Add("pointer").Add("index", int.MinValue, int.MaxValue, true).Add("", "=").AddTrailing("item");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            //If the user is creating a list
            if (args[0] == "create")
            {
                //Generate a random universaly unique identifier
                int uuid;
                do
                {
                    uuid = rng.Next();
                } while (Registry.ContainsKey(uuid));

                //Create a list with given id
                Registry.Add(uuid, new List<string>());

                //If the user passed a weak array, convert it to a list
                if (args.Count > 1)
                {
                    Registry[uuid].AddRange(args.Skip(1));
                }

                //Returns the ID
                target.WriteLine("[[" + ConConverter.ToString(uuid) + "]]");
            }
            else
            {
                //Gets the list with the given ID
                List<string> l = ConConverter.GetList(args[0]);

                //If the user attempts indexing (the second argument is integer)
                if (args.IsInteger(1))
                {
                    //Gets the index
                    int index = GetIndex(args[1], l);

                    //If this is an write
                    if (args.Count > 2)
                    {
                        l[index] = args.JoinEnd(3);
                    }
                    //If this is a read
                    else
                    {
                        target.WriteLine(l[index]);
                    }
                }

                //If the user attempt to use a subcommand
                switch (args[1])
                {
                    case "add":
                        l.Add(args.JoinEnd(2));
                        break;
                    case "remove":
                        l.Remove(args.JoinEnd(2));
                        break;
                    case "removeat":
                        l.RemoveAt(GetIndex(args[2], l));
                        break;
                    case "clear":
                        l.Clear();
                        break;
                    case "count":
                        target.WriteLine(l.Count);
                        break;
                    case "addrange":
                        l.AddRange(args.Skip(2));
                        break;
                    case "contains":
                        target.WriteLine(l.Contains(args.JoinEnd(2)));
                        break;
                    case "flush":
                        foreach (KeyValuePair<int, List<string>> i in Registry)
                        {
                            if (i.Value == l)
                            {
                                Registry.Remove(i.Key);
                                break;
                            }
                        }
                        
                        break;
                    case "sort":
                        switch (args[2])
                        {
                            case "a":
                                l.Sort();
                                break;
                            case "z":
                                l.Sort();
                                l.Reverse();
                                break;
                            case "1":
                                l.OrderBy(i => ConConverter.ToFloat(i));
                                break;
                        }
                        break;
                    case "reverse":
                        l.Reverse();
                        break;
                    case "toarray":
                        target.WriteLine(string.Join(" ", l.ToArray()));
                        break;
                    case "insert":
                        l.Insert(GetIndex(args[2], l), args.JoinEnd(3));
                        break;
                    case "insertrange":
                        l.InsertRange(GetIndex(args[2], l), args.Skip(3));
                        break;
                }
            }
        }

        static int GetIndex(string raw, List<string> target)
        {
            //Gets the index the user specifies
            int index = ConConverter.ToInt(raw).Value;

            //If the index is less than zero, attempt reversed indexing
            if (index < 0)
            {
                index = target.Count + index;

                //If the index is STILL less than zero, throw an error
                if (index < 0)
                {
                    ThrowOutOfRangeError(ConConverter.ToInt(raw).Value, -target.Count, target.Count - 1, ErrorCode.NUMBER_TOO_SMALL);
                }
            }

            //If the user tries to index an empty list
            if (target.Count == 0)
            {
                ThrowGenericError("List is empty", ErrorCode.INVALID_CONTEXT);
            }

            //If the user tries to index a position that is too big
            if (index >= target.Count)
            {
                ThrowOutOfRangeError(index, 0, target.Count - 1, ErrorCode.NUMBER_TOO_BIG);
            }

            return index;
        }
    }
}
