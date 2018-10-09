using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace UserConsoleLib
{
    /// <summary>
    /// Represents an executable command
    /// </summary>
    public abstract class Command : IComparable<Command>
    {
        /// <summary>
        /// Gets the name of the command
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description text for this command
        /// </summary>
        public abstract string HelpDescription { get; }

        /// <summary>
        /// The method to be executed when the command is invoked
        /// </summary>
        /// <param name="args">Arguments passed to this command call</param>
        /// <param name="target">Writable console output</param>
        protected abstract void Executed(Params args, IConsoleOutput target);

        /// <summary>
        /// Gets the syntax of the command
        /// </summary>
        /// <returns></returns>
        public abstract Syntax GetSyntax(Params args);

        /// <summary>
        /// Does this command start or stop a code-block?
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsCodeBlockCommand()
        {
            return false;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="arguments">Arguments to pass to the command</param>
        /// <param name="target">Writable console target</param>
        public int Execute(IEnumerable<string> arguments, IConsoleOutput target)
        {
            try
            {
                //If the user attempt to use commands like  if, while and for outside a script context
                if (IsCodeBlockCommand() && !(target is ScriptTargetWrapper))
                {
                    ThrowGenericError("This command can only be used in script files", ErrorCode.INVALID_CONTEXT);
                }

                Params p = new Params(arguments);
                GetSyntax(p).IsCorrectSyntax(this, p, true);

                Executed(p, target);
                return 0;
            }
            catch (CommandException x)
            {
                foreach (string i in x.Message.Split('\n'))
                {
                    target.WriteError(i);
                }

                return x.ErrorCode;
            }
            
        }

        //BM Command Registry

        /// <summary>
        /// Houses all registered commands
        /// </summary>
        internal static List<Command> AllCommandsInternal { get; } = new List<Command>()
        {
            new StandardLib.Echo(),
            new StandardLib.Clear(),
            new StandardLib.Help(),
            new StandardLib.Exit(),
            new StandardLib.Math.Abs(),
            new StandardLib.Math.Cos(),
            new StandardLib.Math.E(),
            new StandardLib.Math.Max(),
            new StandardLib.Math.Min(),
            new StandardLib.Math.Operation(),
            new StandardLib.Math.Pi(),
            new StandardLib.Math.Sign(),
            new StandardLib.Math.Sin(),
            new StandardLib.Math.Tan(),
            new StandardLib.Math.Average(),
            new StandardLib.Math.Sum(),
            new StandardLib.Randomness.Random(),
            new StandardLib.Randomness.Choose(),
            new StandardLib.Variables.Get(),
            new StandardLib.Variables.Set(),
            new StandardLib.Variables.List(),
            new StandardLib.Variables.Range(),
            new StandardLib.Control.If(),
            new StandardLib.Control.EndBlock(),
            new StandardLib.Control.Load(),
            new StandardLib.Control.And(),
            new StandardLib.Control.Or(),
            new StandardLib.Control.Boolean(),
            new StandardLib.Control.Not(),
            new StandardLib.Control.Skip(),
            new StandardLib.Control.Label(),
            new ExtendedLib.IO.Cd(),
            new ExtendedLib.IO.Dir(),
            new ExtendedLib.IO.File(),
            new ExtendedLib.IO.Directory(),
            new StandardLib.Control.DebugBreak(),
            new StandardLib.Control.While(),
            new StandardLib.Control.For(),
            new StandardLib.Variables.String(),
            new StandardLib.Structure.Function(),
            new StandardLib.Breaking.Endscript(),
            new StandardLib.Math.Floor(),
            new StandardLib.Math.Ceiling(),
            new StandardLib.Math.Sqrt(),
            new StandardLib.Breaking.Break(),
            new StandardLib.Breaking.Continue(),
            new StandardLib.Brackets.BracketClose(),
            new StandardLib.Brackets.BracketOpen(),
            new StandardLib.Brackets.Brac(),
            new ExtendedLib.DateTime.Time(),
            new ExtendedLib.DateTime.Date(),
        };

        /// <summary>
        /// Gets all commands registered
        /// </summary>
        public static System.Collections.ObjectModel.ReadOnlyCollection<Command> AllCommands
        {
            get
            {
                return AllCommandsInternal.AsReadOnly();
            }
        }

        /// <summary>
        /// Registers a command in this command collection
        /// </summary>
        /// <param name="command">Command to register</param>
        public static void Register(Command command)
        {
            if (AllCommandsInternal.Any(i => i.Name.ToLower() == command.Name.ToLower()))
            {
                throw new ArgumentException("A command with this name has allready been registered");
            }

            AllCommandsInternal.Add(command);
        }

        /// <summary>
        /// Finds and returns a command by name
        /// </summary>
        /// <param name="name">Name of command</param>
        /// <returns></returns>
        public static Command GetByName(string name)
        {
            name = name.Trim();

            return AllCommandsInternal.Where(i => i.Name.ToLower() == name.ToLower()).SingleOrDefault();
        }

        /// <summary>
        /// Parses and invokes a command line entry
        /// </summary>
        /// <param name="line">Line to parse</param>
        /// <param name="target">Writable console target</param>
        public static int ParseLine(string line, IConsoleOutput target)
        {
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }

            return Parseline(line.GetEnumerator(), target);





            //string[] escapedList = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+").Cast<Match>().Select(m => m.Value).ToArray();

            //Command cmd = GetByName(escapedList[0]);

            //if (cmd == null)
            //{
            //    target.WriteError("No such command with name '" + escapedList[0] + "'");
            //    return -1;
            //}

            //return cmd.Execute(escapedList.Skip(1), target);

        }
   
        static int Parseline(IEnumerator<char> e, IConsoleOutput target)
        {
            VariableCollection vars;

            if (target is ScriptTargetWrapper v && v.Session.Scope.Any())
            {
                vars = v.Session.Scope.Peek().Locals;
            }
            else
            {
                vars = VariableCollection.GlobalVariables;
            }

            List<string> _ = new List<string>
            {
                ""
            };

            while (e.MoveNext())
            {
                if (e.Current == '(')
                {
                    StringInterface i = new StringInterface();
                    int outputCode = Parseline(e, i);

                    if (outputCode != 0)
                    {
                        target.WriteError("Internal " + i.Last());
                        return outputCode;
                    }

                    _ = _.Concat((i.LastOrDefault() ?? "").Split(' ')).ToList();
                }
                else if (e.Current == ' ')
                {
                    _.Add("");
                }
                else if (e.Current == ')')
                {
                    break;
                }
                else
                {
                    _[_.Count - 1] += e.Current;
                }
            }

            _.RemoveAll(i => string.IsNullOrEmpty(i));
            _[0] = _[0].TrimStart();

            Command cmd = GetByName(_[0]);

            //If command was not found, or wasn't a command
            if (cmd == null)
            {
                //If the 'command name' was a valid number, it must be an arithmeric operation
                if (double.TryParse(_[0], out double i))
                {
                    cmd = GetByName("op");
                    _.Insert(0, "op");
                }
                //If the 'command name' was the name of a variable, it must be a set or get operation
                else if (vars.IsDefined(_[0].TrimStart('$')))
                {
                    //If there are no arguments to the command, it must be a get operation
                    if (_.Count == 1)
                    {
                        cmd = GetByName("get");
                        _ = new List<string>() { "get", _[0].TrimStart('$') };
                    }
                    //If there are arguments to the command, it must be a set operation
                    else
                    {
                        cmd = GetByName("set");

                        IEnumerable<string> _2 = _.AsEnumerable();
                        _ = new List<string>() { "get", _[0].TrimStart('$') };
                        _.AddRange(_2.Skip(1));

                    }
                }
                else
                {
                    if (_[0].StartsWith("$"))
                    {
                        target.WriteError("No such variable with name '" + _[0].TrimStart('$') + "'");
                    }
                    else
                    {
                        target.WriteError("No such command with name '" + _[0] + "'");
                    }
                    
                    return -1;
                }
            }

            return cmd.Execute(_.Skip(1), target);


        }

        #region Exception throwing

        /// <summary>
        /// Throws an error indicating that the command was used incorrectly
        /// </summary>
        /// <param name="cmd">Command in question</param>
        /// <param name="args">Arguments of command in question</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowSyntaxError(Command cmd, Params args, int errorCode)
        {
            throw new CommandException("Syntax:\n" + cmd.GetSyntax(args).CreateSyntaxString().Replace("\n", "\n*   ").Insert(0, "*   "), errorCode);
        }

        /// <summary>
        /// Throws an error indicating that one of the arguments passed to a command was not a number when it should have been
        /// </summary>
        /// <param name="incorrectValue">String that failed to be recognized as a number</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowNaNError(string incorrectValue, int errorCode)
        {
            throw new CommandException("Argument '" + incorrectValue + "' was expected to be a number, but wasn't", errorCode);
        }

        /// <summary>
        /// Throws an error indicating that an argument was unexpected
        /// </summary>
        /// <param name="argumentValue">Value of unexpected argument</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowArgumentError(string argumentValue, int errorCode)
        {
            throw new CommandException("Argument '" + argumentValue + "' was unexpected", errorCode);
        }

        /// <summary>
        /// Throws an error indicating that an argument did not fall under any valid values
        /// </summary>
        /// <param name="argumentValue">Value of incorrect argument</param>
        /// <param name="expectedValues">Allowed arguments</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowArgumentError(string argumentValue, IEnumerable<string> expectedValues, int errorCode)
        {
            throw new CommandException("Argument '" + argumentValue + "' was unexpected. Valid values: " + string.Join(", ", expectedValues.ToArray()), errorCode);
        }

        /// <summary>
        /// Throws an error indicating that a number was outside the range of valid values
        /// </summary>
        /// <param name="value">Value that was out of range</param>
        /// <param name="minValue">Minimum expected value.</param>
        /// <param name="maxValue">Maximum expected value.</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowOutOfRangeError(double value, double minValue, double maxValue, int errorCode)
        {
            if (double.IsNaN(minValue) || double.IsNaN(maxValue))
            {
                throw new ArgumentNullException("", "NaN not supported for min or max value");
            }

            if (value > minValue)
            {
                throw new CommandException("The number " + value + " is too big. It must at most be " + maxValue, errorCode);
            }
            else if (value < maxValue)
            {
                throw new CommandException("The number " + value + " is too small. It must at least be " + minValue, errorCode);
            }
        }

        /// <summary>
        /// Throws an error indicating that a number was non-integer
        /// </summary>
        /// <param name="incorrectValue">Value that was non-integer</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowNoFloatsAllowedError(double incorrectValue, int errorCode)
        {
            throw new CommandException("The value " + incorrectValue + " must be an integer", errorCode);
        }

        /// <summary>
        /// Throws a generic error message
        /// </summary>
        /// <param name="message">Message to throw</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowGenericError(string message, int errorCode)
        {
            throw new CommandException(message, errorCode);
        }

        /// <summary>
        /// Returns an error code without showing any error messages
        /// </summary>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowSilentError(int errorCode)
        {
            throw new CommandException("", errorCode);
        }

        #endregion

        /// <summary>
        /// Returns the string representation of this command
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[Command: " + Name + "]";
        }

        /// <summary>
        /// Compares the name of this command to another command's name, allowing alphabethic soring
        /// </summary>
        /// <param name="other">Command to compare to</param>
        /// <returns></returns>
        public int CompareTo(Command other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
