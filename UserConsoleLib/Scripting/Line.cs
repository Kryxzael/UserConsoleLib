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
        public string Command { get; set; }

        /// <summary>
        /// The parameters of the command
        /// </summary>
        public Params Parameters { get; set; }

        /// <summary>
        /// Converts the given string into a LineCommand instance
        /// </summary>
        /// <param name="raw"></param>
        public Line(string raw)
        {
            //Trim line of any prefixes or sufixes and newline characters
            raw = raw
                .Trim()
                .Replace("\r", "")
                .Replace("\n", "");

            if (raw.StartsWith("(") && raw.EndsWith(")"))
            {
                raw = raw.Substring(1, raw.Length - 2);
            }


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
        /// Is the command portion of this line a subexpression?
        /// </summary>
        /// <returns></returns>
        public bool CommandIsSubExpression()
        {
            return Command.StartsWith("(");
        }

        /// <summary>
        /// Is the command portion of this code block?
        /// </summary>
        /// <returns></returns>
        public bool CommandIsCodeBlock()
        {
            return Command.StartsWith("{");
        }

        /// <summary>
        /// Is the command portion of this line a subexpression or code block?
        /// </summary>
        /// <returns></returns>
        public bool CommandIsBlock()
        {
            return CommandIsSubExpression() || CommandIsCodeBlock();
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
                        if (!levels.Any())
                            throw new CommandException("Syntax error: Unexpected token ')'", ErrorCode.INTERNAL_ERROR);

                        if (levels.Pop() != '(')
                        {
                            throw new CommandException("Syntax error: Expected ')'", ErrorCode.INTERNAL_ERROR);
                        }
                        break;
                    case '}':
                        if (!levels.Any())
                            throw new CommandException("Syntax error: Unexpected token '}'", ErrorCode.INTERNAL_ERROR);

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

        /// <summary>
        /// Returns the line as a normalized string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Command + " " + string.Join(" ", Parameters.ToArray());
        }

    }
}
