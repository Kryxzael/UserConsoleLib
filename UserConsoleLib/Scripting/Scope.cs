using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserConsoleLib.Scripting;

namespace UserConsoleLib
{
    /// <summary>
    /// Represents a code block with its own variable collection, that can be executed sequentially
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// The top-level scope. Everything defined here is available to all subscopes
        /// </summary>
        public static readonly Scope GlobalScope = new Scope();

        /// <summary>
        /// Gets a collection of this scopes imediate variables
        /// </summary>
        public VariableCollection Variables { get; }

        /// <summary>
        /// Gets the lines of commands this scope contains
        /// </summary>
        public Lines Lines { get; }

        /// <summary>
        /// Defines a new scope with the given parent scope and lines
        /// </summary>
        /// <param name="parentScope"></param>
        public Scope(Scope parentScope)
        {
            Variables = new VariableCollection(parentScope.Variables);
        }

        /// <summary>
        /// Defines a new top level scope
        /// </summary>
        public Scope()
        {
            Variables = new VariableCollection();
        }

        /// <summary>
        /// Parses a command in the given scope
        /// </summary>
        /// <param name="line"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int ParseLine(string line, IConsoleOutput target)
        {
            return ParseLine(new Line(line), target);
        }

        /// <summary>
        /// Parses a command in the given scope
        /// </summary>
        public int ParseLine(Line line, IConsoleOutput target)
        {
            /*
             * Reversed recursive evaluation
             */
            for (int i = 0; i < line.Parameters.Count; i++)
            {
                //Parameter i is a subexpression and must be evaluated
                if (line.ParamIsSubExpression(i))
                {
                    //Runs subexpression and stores its result in 'pipe'
                    StringInterface pipe = new StringInterface();
                    ParseLine(new Line(line.Parameters[i].TrimStart('(').TrimEnd(')')), pipe);

                    //Parameter is update to the output of the evaluated subexpression
                    line.Parameters[i] = pipe.LastOrDefault();
                }
            }

            /*
             * Find and execute command
             */
            Command cmd = Command.GetByName(line.Command);

            //Command is not a command but may still be syntax sugar
            if (cmd == null)
            {
                //Command is number => Arithmetic operation
                if (ConConverter.ToDouble(line.Command) != null)
                {
                    return Command.GetByType<StandardLib.Math.Operation>().Execute(
                        arguments: new string[] { line.Command }.Concat(line.Parameters), 
                        target: target,
                        scope: this
                    );
                }

                //Command is known variable in scope, variable operation
                else if (Variables.IsDefined(line.Command.TrimStart('$')))
                {
                    return Command.GetByType<StandardLib.Variables.Varop>().Execute(
                        arguments: new string[] { line.Command }.Concat(line.Parameters),
                        target: target,
                        scope: this
                    );
                }

                //Invalid command
                else
                {
                    target.WriteError(line.Command + " is not a command or defined variable");

                    return -1;
                }
            }

            //Command is found
            else
            {
                return cmd.Execute(line.Parameters, target, this);
            }
        }

        /// <summary>
        /// Creates a scope from the input buffer. This can be a single line or a code block made with braces
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Scope FromBuffer(string[] input)
        {
            throw new NotImplementedException();
        }
    }
}
