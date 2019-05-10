using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.Scripting
{
    /// <summary>
    /// Represents a single-line command
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// The command portion of the line
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// The parameters of the command
        /// </summary>
        public Params Parameters { get; }

        /// <summary>
        /// Converts the given string into a LineCommand instance
        /// </summary>
        /// <param name="raw"></param>
        public Line(string raw)
        {
            //Trim line of any prefixes or sufixes
            raw = raw.Trim();

            //Extract the command
            Command = FindFirstCommandComponent(raw);
            raw = raw.Substring(Command.Length);

            //Extract the parameters
            List<string> parameters = new List<string>();
            while (raw.Length > 0)
            {
                parameters.Add(FindFirstCommandComponent(raw));
                raw = raw.Substring(parameters.Last().Length + 1);
            }
            Parameters = new Params(parameters);
        }

        /// <summary>
        /// Is the parameter at the given index a subexpression
        /// </summary>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        public bool ParamIsSubExpression(int paramIndex)
        {
            return Parameters[paramIndex]?.StartsWith("(") == true;
        }

        /// <summary>
        /// Is the parameter at the given index a code block
        /// </summary>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        public bool ParamIsCodeBlock(int paramIndex)
        {
            return Parameters[paramIndex]?.StartsWith("{") == true;
        }

        /// <summary>
        /// Is the parameter at the given index a subexpression or code block
        /// </summary>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        public bool ParamIsBlock(int paramIndex)
        {
            return ParamIsCodeBlock(paramIndex) || ParamIsSubExpression(paramIndex);
        }

        /// <summary>
        /// Finds and returns the first command/argument of the given string
        /// </summary>
        /// <param name="input">A command string</param>
        /// <returns></returns>
        private static string FindFirstCommandComponent(string input)
        {
            //Trim input
            input = input.Trim();
            StringBuilder output = new StringBuilder();

            //Stores the scopes of the line
            Stack<char> levels = new Stack<char>();

            //Enumerate every char in string
            foreach (char c in input)
            {
                switch (c)
                {
                    //New scope, push it
                    case '(':
                    case '{':
                        levels.Push(c);
                        break;

                    //Close scope
                    case ')':
                        if (levels.Pop() != '(')
                        {
                            throw new CommandException("Syntax error: Expected ')'", ErrorCode.INTERNAL_ERROR);
                        }
                        break;
                    case '}':
                        if (levels.Pop() != '{')
                        {
                            throw new CommandException("Syntax error: Expected '}'", ErrorCode.INTERNAL_ERROR);
                        }
                        break;
                }

                //We have reached the end of the component. Break function
                if (c == ' ' && !levels.Any())
                {
                    break;
                }

                //Append char to output
                output.Append(c);
            }

            return output.ToString();
        }

    }
}
