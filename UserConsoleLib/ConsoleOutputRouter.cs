using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace UserConsoleLib
{
    /// <summary>
    /// Advanced console output router
    /// </summary>
    public class ConsoleOutputRouter : IConsoleOutput, IEnumerable<IConsoleOutput>, ICloneable
    {
        /// <summary>
        /// Gets the amount of listeners this ConsoleOutputRouter has registered
        /// </summary>
        public int Count => TargetsInternal.Count;

        /// <summary>
        /// Internal list of targets
        /// </summary>
        internal List<RouterItem> TargetsInternal { get; } = new List<RouterItem>();

        /// <summary>
        /// Gets an enumerable set of all targets of this ConsoleOutputRouter
        /// </summary>
        public ReadOnlyCollection<IConsoleOutput> Targets => TargetsInternal.Select(i => i.Target).ToList().AsReadOnly();

        /// <summary>
        /// Adds a listener
        /// </summary>
        /// <param name="target">Target output device</param>
        /// <param name="options">Filtration options</param>
        /// <param name="filterItems">Keywords to filter out (or include)</param>
        public void AddListener(IConsoleOutput target, RouterOptions options, IEnumerable<string> filterItems)
        {
            if (TargetsInternal.Where(i => i.Target == target).Count() > 0)
            {
                throw new ArgumentException(nameof(target), "This " + nameof(IConsoleOutput) + " is allready registered");
            }

            if (!options.HasFlag(RouterOptions.ReceivesErrors) && !options.HasFlag(RouterOptions.ReceivesWarnings) && !options.HasFlag(RouterOptions.ReceivesMessages))
            {
                throw new ArgumentException(nameof(options), "This listener has no target streams");
            }

            TargetsInternal.Add(new RouterItem() { Target = target, Options = options, ExcludedKeywords = filterItems.ToArray() });
        }

        /// <summary>
        /// Removes a listener
        /// </summary>
        /// <param name="target">Listener to remove</param>
        /// <returns></returns>
        public bool RemoveListener(IConsoleOutput target)
        {
            return TargetsInternal.RemoveAll(i => i.Target == target) != 0;
        }

        /// <summary>
        /// Clears the buffers of all listeners that has the option flag CanClear
        /// </summary>
        public void ClearBuffer()
        {
            TargetsInternal.ForEach(i => i.ClearBuffer());
        }

        /// <summary>
        /// Gets the public enumerator of all output devices connected to this ConsoleOutputRouter
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IConsoleOutput> GetEnumerator()
        {
            return Targets.GetEnumerator();
        }

        /// <summary>
        /// Writes an error message to all listeners that has the option flag ReceiveErrors
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteError(string message)
        {
            TargetsInternal.ForEach(i => i.WriteError(message));
        }

        /// <summary>
        /// Writes a message to all listeners that has the option flag ReceiveMessages
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteLine(string message)
        {
            TargetsInternal.ForEach(i => i.WriteLine(message));
        }

        /// <summary>
        /// Writes an warning message to all listeners that has the option flag ReceiveWarnings
        /// </summary>
        /// <param name="message">Message to write</param>
        public void WriteWarning(string message)
        {
            TargetsInternal.ForEach(i => i.WriteWarning(message));
        }

        /// <summary>
        /// Gets the public enumerator of all output devices connected to this ConsoleOutputRouter
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Targets.GetEnumerator();
        }

        /// <summary>
        /// Returns the string representation of this router
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Command out -> " + TargetsInternal.Count + " targets";
        }

        /// <summary>
        /// Creates an exact duplicate of this ConsoleOutputRouter
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ConsoleOutputRouter _ = new ConsoleOutputRouter();
            _.TargetsInternal.AddRange(TargetsInternal);
            return _;
        }
    }

    /// <summary>
    /// Represents a set of options that can be applied to listeners in ConsoleOutputRouters
    /// </summary>
    [Flags]
    public enum RouterOptions
    {
        /// <summary>
        /// No options are enabled, this option type is invalid
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Indicates that the listener should print any non-error non-warning messages sent to it
        /// </summary>
        ReceivesMessages = 1,

        /// <summary>
        /// Indicates that the listener should print any warning messages sent to it
        /// </summary>
        ReceivesWarnings = 2,

        /// <summary>
        /// Indicates that the listener should print any error messages sent to it
        /// </summary>
        ReceivesErrors = 4,

        /// <summary>
        /// Indicates that the listener can be cleared using ClearBuffer
        /// </summary>
        CanClear = 8,

        /// <summary>
        /// Indicates that only messages containing words from the filtered words list should be printed
        /// </summary>
        InclusiveKeywords = 16,

        /// <summary>
        /// Indicates that the listener should print any message type sent to it
        /// </summary>
        ReceivesAll = ReceivesMessages | ReceivesWarnings | ReceivesErrors,

        /// <summary>
        /// Indicates that the listener should print any non-ok messages
        /// </summary>
        ReceivesNonMessages = ReceivesWarnings | ReceivesErrors,
    }

    internal class RouterItem : IConsoleOutput
    {
        /// <summary>
        /// Target output of router item
        /// </summary>
        public IConsoleOutput Target { get; set; }

        /// <summary>
        /// Keywords to be filtered away from the target
        /// </summary>
        public string[] ExcludedKeywords { get; set; }

        /// <summary>
        /// Gets or sets the option flags for this routeritem
        /// </summary>
        public RouterOptions Options { get; set; }

        public void ClearBuffer()
        {
            if (Options.HasFlag(RouterOptions.CanClear))
            {
                Target.ClearBuffer();
            }
        }

        public void WriteError(string message)
        {
            if (Options.HasFlag(RouterOptions.ReceivesErrors) && ShouldWrite(message))
            {
                Target.WriteError(message);
            }
        }

        public void WriteLine(string message)
        {
            if (Options.HasFlag(RouterOptions.ReceivesMessages) && ShouldWrite(message))
            {
                Target.WriteLine(message);
            }
        }

        public void WriteWarning(string message)
        {
            if (Options.HasFlag(RouterOptions.ReceivesWarnings) && ShouldWrite(message))
            {
                Target.WriteWarning(message);
            }
        }

        bool ShouldWrite(string value)
        {
            foreach (string i in ExcludedKeywords)
            {
                if (value.ToLower().Contains(i.ToLower()))
                {
                    return Options.HasFlag(RouterOptions.InclusiveKeywords);
                }
            }

            return !Options.HasFlag(RouterOptions.InclusiveKeywords);
        }
    }

    internal static class HasFlagWrapper
    {
        public static bool HasFlag(this RouterOptions ro, RouterOptions flag)
        {
            return (ro & flag) == flag;
        }
    }
}
