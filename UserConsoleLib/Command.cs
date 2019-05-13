using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UserConsoleLib.Scripting;

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
        /// <param name="scope">The scope the command was executed from</param>
        protected abstract void Executed(Params args, IConsoleOutput target, Scope scope);

        /// <summary>
        /// Can you run this command from the console?
        /// </summary>
        public bool IsEnabled { get; private set; } = true;
		
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
        /// <param name="scope">The scope to execute this function in</param>
        public int Execute(IEnumerable<string> arguments, IConsoleOutput target, Scope scope)
        {
            try
            {
                //If the user attempt to use commands like  if, while and for outside a script context
                if (IsCodeBlockCommand())
                {
                    ThrowGenericError("This command can only be used in script files", ErrorCode.INVALID_CONTEXT);
                }

                Params p = new Params(arguments);
                GetSyntax(p).IsCorrectSyntax(this, p, true);

                Executed(p, target, scope);
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
        internal static List<Command> AllCommandsInternal { get; } = new List<Command>();

        private static Command[] _systemCommands = new Command[]
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
            new StandardLib.Variables.Var(),
            new StandardLib.Variables.Varop(),
            new StandardLib.Variables.List(),
            new StandardLib.Variables.RangeCommand(),
            new StandardLib.Control.And(),
            new StandardLib.Control.Or(),
            new StandardLib.Control.Boolean(),
            new StandardLib.Control.Not(),
            new ExtendedLib.IO.Cd(),
            new ExtendedLib.IO.Dir(),
            new ExtendedLib.IO.File(),
            new ExtendedLib.IO.Directory(),
            new StandardLib.Variables.String(),
            new StandardLib.Math.Floor(),
            new StandardLib.Math.Ceiling(),
            new StandardLib.Math.Sqrt(),
            new StandardLib.Brackets.BracketClose(),
            new StandardLib.Brackets.BracketOpen(),
            new StandardLib.Structure.Load(),
            new StandardLib.Structure.If(),

            new StandardLib.Brackets.Brac(),
            new ExtendedLib.DateTime.Time(),
            new ExtendedLib.DateTime.Date(),
		};
		
		static Command()
		{
            //Registers all system commands
			foreach (Command i in _systemCommands)
            {
                Register(i);
            }
		}

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
            AllCommandsInternal.Add(command);
            command.IsEnabled = true;
        }
		
		/// <summary>
        /// Registers a command in this command collection
        /// </summary>
        /// <typeparam name="C">Type of command to instantiate and register</typeparam>
		public static void Register<C>() where C : Command, new()
		{
			Register(new C());
		}

        /// <summary>
        /// Disables usage of a command without deregistering it
        /// </summary>
        /// <param name="command">Command to disable</param>
        public static void DisableCommand(Command command)
        {
            command.IsEnabled = false;
        }

        /// <summary>
        /// Enables a command previously disabled by DisableCommand()
        /// </summary>
        /// <param name="command">Command to enable</param>
        public static void EnableCommand(Command command)
        {
			command.IsEnabled = true;
        }
		
		/// <summary>
        /// Disables all built-in commands
        /// <param name="disableHelp">Will the help command also be disabled?</param>
        /// </summary>
		public static void DisableSystemCommands(bool disableHelp = true)
		{
			foreach (Command i in _systemCommands) 
			{
				if (!disableHelp && i.GetType() == typeof(StandardLib.Help))
				{
					continue;
				}
				
				DisableCommand(i);
			}
		}
		
		/// <summary>
        /// Enables all built-in commands
        /// </summary>
		public static void EnableSystemCommands()
		{
			foreach (Command i in _systemCommands) 
			{
				EnableCommand(i);
			}
		}

        /// <summary>
        /// Finds and returns a command by name
        /// </summary>
        /// <param name="name">Name of command</param>
        /// <returns></returns>
        public static Command GetByName(string name)
        {
            name = name.Trim();
            return AllCommandsInternal.Where(i => i.Name.ToUpper() == name.ToUpper() && i.IsEnabled).LastOrDefault();
        }
		
		/// <summary>
        /// Finds and returns a command by its typecode
        /// </summary>
        /// <typeparam name="C">Type of command to find</typeparam>
        /// <returns></returns>
		public static Command GetByType<C>() where C : Command 
		{
			return AllCommandsInternal.Where(i => i.GetType() == typeof(C)).LastOrDefault();
		}
		
		/// <summary>
        /// Finds and returns a command by name
        /// </summary>
        /// <param name="name">Name of command</param>
		/// <param name="includeDisabled">Will the search include commands that have been disabled?</param>
        /// <returns></returns>
		public static Command GetByName(string name, bool includeDisabled)
		{
			if (!includeDisabled)
			{
				return GetByName(name);
			}
			
			name = name.Trim();
            return AllCommandsInternal.Where(i => i.Name.ToUpper() == name.ToUpper()).LastOrDefault();
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
        /// <param name="intendedRange">Expected range of the argument</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        protected internal static void ThrowOutOfRangeError(double value, Range intendedRange, int errorCode)
        {
            if (double.IsNaN(intendedRange.Minimum) || double.IsNaN(intendedRange.Maximum))
            {
                throw new ArgumentNullException("", "NaN not supported for min or max value");
            }

            if (value > intendedRange.Maximum)
            {
                throw new CommandException("The number " + value + " is too big. It must at most be " + intendedRange.Maximum, errorCode);
            }
            else if (value < intendedRange.Minimum)
            {
                throw new CommandException("The number " + value + " is too small. It must at least be " + intendedRange.Minimum, errorCode);
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
