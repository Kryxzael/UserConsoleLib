using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    /// <summary>
    /// Interfaces with the native windows console, usefull for console applications
    /// </summary>
    public class ConsoleInterface : IConsoleOutput
    {
        /// <summary>
        /// Clears the console
        /// </summary>
        public void ClearBuffer()
        {
            Console.Clear();
        }

        /// <summary>
        /// Writes an error message to the console's error stream
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteError(string message)
        {
            ConsoleColor c = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = c;
        }

        /// <summary>
        /// Writes a message to the console's default stream
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes a warning message to the console's error stream 
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteWarning(string message)
        {
            ConsoleColor c = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = c;
        }

        /// <summary>
        /// Reads a line of text and parses it using the source of the ConsoleInterface object
        /// </summary>
        /// <param name="target">A writable console target to write output to</param>
        /// <returns></returns>
        public int ReadLine(IConsoleOutput target)
        {
            Console.Write("> ");

            return Command.ParseLine(Console.ReadLine(), target);
        }

        /// <summary>
        /// Reads a line of text and parses it using the source of the ConsoleInterface object
        /// </summary>
        /// <returns></returns>
        public int ReadLine()
        {
            return ReadLine(this);
        }

        /// <summary>
        /// Returns the string represenation of this ConsoleInterface
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Command out -> console";
        }
    }
}
