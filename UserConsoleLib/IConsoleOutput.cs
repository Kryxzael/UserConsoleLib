using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    /// <summary>
    /// Represents an output device that can be written to
    /// </summary>
    public interface IConsoleOutput
    {
        /// <summary>
        /// Writes a line of text to the console
        /// </summary>
        /// <param name="message">Text to write</param>
        void WriteLine(string message);

        /// <summary>
        /// Writes an error message to the console
        /// </summary>
        /// <param name="message">Error to write</param>
        void WriteError(string message);

        /// <summary>
        /// Writes a warning message to the console
        /// </summary>
        /// <param name="message">Warning to write</param>
        void WriteWarning(string message);

        /// <summary>
        /// Clears the buffer of the console
        /// </summary>
        void ClearBuffer();
    }
}
