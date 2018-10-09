using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib
{
    /// <summary>
    /// Represents an exepction that can be handled by command execution runtime
    /// </summary>
    internal class CommandException : Exception
    {
        /// <summary>
        /// Error code of exception
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// Initializes a new command exception
        /// </summary>
        /// <param name="message">Message to display to output device</param>
        /// <param name="errorCode">Non-0-non-negative error code</param>
        public CommandException(string message, int errorCode) : base(message)
        {
            if (errorCode <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(errorCode), "Error code cannot be 0 or negative");
            }

            ErrorCode = errorCode;
        }
    }
}
