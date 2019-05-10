using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.Scripting
{
    /// <summary>
    /// Represents a list of commands
    /// </summary>
    public class Lines : IEnumerable<Line>
    {
        private Line[] _internal;

        /// <summary>
        /// Gets the amount of lines this Lines instance has
        /// </summary>
        public int Count => _internal.Length;

        /// <summary>
        /// Creates a new LineCommand collection from the given strings
        /// </summary>
        /// <param name="lines"></param>
        public Lines(IEnumerable<string> lines) : this(lines.Select(i => new Line(i)))
        {
        }

        /// <summary>
        /// Creates a new LineCommand collection from the given LineCommands
        /// </summary>
        /// <param name="lines"></param>
        public Lines(IEnumerable<Line> lines)
        {
            _internal = lines.ToArray();
        }

        /// <summary>
        /// Gets the command at the given line (starting at 1)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public Line GetCommandAtLine(int line)
        {
            return _internal[line - 1];
        }

        /// <summary>
        /// Gets the command at the given index (starting at 0)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public Line GetCommandAtIndex(int line)
        {
            return _internal[line];
        }

        /// <summary>
        /// Gets the enumerator of this Lines instance
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Line> GetEnumerator()
        {
            return _internal.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internal.GetEnumerator();
        }
    }
}
