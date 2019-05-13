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
        public Lines Lines { get; private set; }

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
            //Command is subexpression and must be evaluated
            //This block is neccessary for commands like 'echo ((random 0 10) * 2)'
            if (line.CommandIsSubExpression())
            {
                //runs subexpression and stores its result in 'pipe'
                StringInterface pipe = new StringInterface();
                ParseLine(line.Command, pipe);
                line.Command = pipe.LastOrDefault();
            }

            for (int i = 0; i < line.Parameters.Count; i++)
            {
                //Parameter i is a subexpression and must be evaluated
                if (line.ParamIsSubExpression(i))
                {
                    //Runs subexpression and stores its result in 'pipe'
                    StringInterface pipe = new StringInterface();
                    ParseLine(line.Parameters[i], pipe);

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
                    return ParseLine("op " + line, target);
                }

                //Command is known variable in scope, variable operation
                else if (Variables.IsDefined(line.Command.TrimStart('$')))
                {
                    return ParseLine("varop " + line, target);
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
        /// Runs every line in this scope sequentialy
        /// </summary>
        /// <param name="target"></param>
        public void RunScope(IConsoleOutput target)
        {
            foreach (Line i in Lines)
            {
                ParseLine(i, target);
            }
        }

        /// <summary>
        /// Creates a scope from the input buffer. This can be a single line or a code block made with braces
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Scope FromLines(string input, Scope parent)
        {
            //Stores the lines as strings. Values are appended char by char to the last element of this list which is why it's initialized with one value
            List<string> lines = new List<string> { "" };

            //Stores the scopes of the iteration
            Stack<char> levels = new Stack<char>();

            //Is the iteration currently inside a code block. Semicolons are not registered while inside these blocks
            bool inSubScope() => levels.Any(i => i == '{');

            //Enumerate every char in string
            foreach (char c in input)
            {
                switch (c)
                {
                    //End of line
                    case ';':
                        //If we are not in a subscope, add a new line
                        if (!inSubScope())
                        {
                            lines.Add("");
                            break;
                        }
                        goto default;
                    //New scope, push it
                    case '(':
                    case '{':
                        levels.Push(c);
                        goto default;

                    //Close scope
                    case ')':
                        if (!levels.Any())
                            throw new CommandException("Syntax error: Unexpected token ')'", ErrorCode.INTERNAL_ERROR);

                        if (levels.Pop() != '(')
                        {
                            throw new CommandException("Syntax error: Expected ')'", ErrorCode.INTERNAL_ERROR);
                        }
                        goto default;
                    case '}':
                        if (!levels.Any())
                            throw new CommandException("Syntax error: Unexpected token '}'", ErrorCode.INTERNAL_ERROR);

                        if (levels.Pop() != '{')
                        {
                            throw new CommandException("Syntax error: Expected '}'", ErrorCode.INTERNAL_ERROR);
                        }

                        //If we are no longer in a subscope. Start the next line
                        if (!inSubScope())
                        {
                            lines[lines.Count - 1] += c;
                            goto case ';';
                        }

                        goto default;

                    //Add the char to the current line
                    default:
                        lines[lines.Count - 1] += c;
                        break;
                }
            }

            //Return the new scope
            return new Scope(parent)
            {
                Lines = new Lines(lines
                    .Where(i => i.Length != 0)
                    .Select(i => new Line(i))
                )
            };   
            
        }
    }
}
