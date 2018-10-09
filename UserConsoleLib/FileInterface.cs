using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace UserConsoleLib
{
    /// <summary>
    /// Allows console log routing to a file
    /// </summary>
    public sealed class FileInterface : IConsoleOutput
    {
        /// <summary>
        /// Path of file to write to
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// If true, this file writer will log the date and time of entry
        /// </summary>
        public bool EnableTimeStamp { get; }

        /// <summary>
        /// Creates a new FileInterface instance
        /// </summary>
        /// <param name="path">Path of file to write to</param>
        /// <param name="enableTimeStamp">If true, this file writer will log the date and time of entry</param>
        public FileInterface(string path, bool enableTimeStamp)
        {
            Path = path;
            EnableTimeStamp = enableTimeStamp;
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        public void ClearBuffer()
        {
            File.WriteAllText(Path, "");
        }

        /// <summary>
        /// Writes an message starting with [ERROR] to the file
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteError(string message)
        {
            File.AppendAllText(Path, CreateTimeStamp() + "[ERROR]".PadRight(8) + message + Environment.NewLine);
        }

        /// <summary>
        /// Writes an message starting with [INFO] to the file
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteLine(string message)
        {
            File.AppendAllText(Path, CreateTimeStamp() + "[INFO]".PadRight(8) + message + Environment.NewLine);
        }

        /// <summary>
        /// Writes an message starting with [WARN] to the file
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteWarning(string message)
        {
            File.AppendAllText(Path, CreateTimeStamp() + "[WARN]".PadRight(8) + message + Environment.NewLine);
        }

        string CreateTimeStamp()
        {
            if (!EnableTimeStamp)
            {
                return "";
            }

            return "[" + 
                DateTime.Now.Year + "." + 
                DateTime.Now.Month.ToString("00") + "." +
                DateTime.Now.Day.ToString("00") + " " + 
                DateTime.Now.Hour.ToString("00") + ":" +
                DateTime.Now.Minute.ToString("00") + ":" +
                DateTime.Now.Second.ToString("00") + 
                "]";
        }

        /// <summary>
        /// Returns the string representation of this FileInterface
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Command out -> " + Path;
        }
    }
}
