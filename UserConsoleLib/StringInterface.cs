using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib
{
    /// <summary>
    /// Represents a console output device that writes to a local string
    /// </summary>
    public class StringInterface : IConsoleOutput, IEnumerable<string>, ICloneable
    {
        /// <summary>
        /// Contains any data written to this IConsoleOutput
        /// </summary>
        public string Data { get; private set; } = "";

        /// <summary>
        /// Delimiter used to seperate individual writing operations
        /// </summary>
        public string Delimiter { get; set; } = Environment.NewLine;

        /// <summary>
        /// Gets the amount of entries written to this StringInterface
        /// </summary>
        public int Count => this.Count();

        /// <summary>
        /// Gets a specific entry of this StringInterface
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index] => this.ElementAt(index);

        /// <summary>
        /// Clears the internal data
        /// </summary>
        public void ClearBuffer()
        {
            Data = "";
        }

        /// <summary>
        /// Writes a message prefixed with 'Error: ' to the internal data 
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteError(string message)
        {
            WriteLine("Error: " + message);
        }

        /// <summary>
        /// Writes a message to the internal data 
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteLine(string message)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                Data += Delimiter;
            }
            Data += message;
        }

        /// <summary>
        /// Writes a message prefixed with 'Warning: ' to the internal data 
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteWarning(string message)
        {
            WriteLine("Warning: " + message);
        }

        /// <summary>
        /// Returns the string representation of this StringInterface
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Command out -> string";
        }

        /// <summary>
        /// Gets the public enumerator of the individual entries of this StringInterface
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return Data.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Creates a clone of this StringInterface with the same data and delimiter settings
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new StringInterface() { Data = this.Data, Delimiter = this.Delimiter };
        }
    }
}
