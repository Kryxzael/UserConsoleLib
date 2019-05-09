using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    /// <summary>
    /// Allows sequential execution off commands from a script file
    /// </summary>
    public class ScriptHost
    {
        internal Stack<ScopeLoopPoint> Scope = new Stack<ScopeLoopPoint>();

        internal bool ShouldBreakDebugger { get; set; }

        /// <summary>
        /// Retreives a list of all statements loaded into this script host
        /// </summary>
        public ReadOnlyCollection<string> Lines { get; set; }

        /// <summary>
        /// Gets the index of the line that was just executed
        /// </summary>
        public int Index { get; set; } = -1;

        ScriptHost(string[] data)
        {
            Lines = data.Where(i => !string.IsNullOrEmpty(i.Trim())).ToList().AsReadOnly();
        }

        /// <summary>
        /// Executes the next line of the script
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool Step(IConsoleOutput output)
        {
            Index++;
            if (Index >= Lines.Count)
            {
                return false;
            }

            Command.ParseLine(Lines[Index], output);

            if (ShouldBreakDebugger)
            {
                Debugger.Break();
            }
            return true;
        }

        /// <summary>
        /// Executes the next line of the script
        /// </summary>
        /// <param name="output">Output device to write to</param>
        /// <param name="errorCode">Variable to store error code of the command executed</param>
        /// <returns></returns>
        private bool Step(IConsoleOutput output, out int errorCode)
        {
            if (!(output is ScriptTargetWrapper))
            {
                output = new ScriptTargetWrapper(output, this);
            }

            Index++;
            if (Index >= Lines.Count)
            {
                errorCode = -1;
                return false;
            }


            //If we are skipping lines and the current line is not a block ender
            if (Scope.Any(i => i.IgnoreLines) && Lines[Index].Trim() != "}")
            {
                Command cmd = Command.GetByName(Lines[Index].Split(' ').First());

                //If the current line is a block command (and not a block ender (see above)), push a new object to the stack
                if (cmd != null && cmd.IsCodeBlockCommand())
                {
                    this.Scope.Push(new ScopeLoopPoint(true, output as ScriptTargetWrapper));
                }

                errorCode = 0;
                return true;
            }

            errorCode = Command.ParseLine(Lines[Index], output);

            if (ShouldBreakDebugger)
            {
                Debugger.Break();
            }
            return true;
        }

        /// <summary>
        /// Batch executes all commands in a script file
        /// </summary>
        /// <param name="output">Output device to write to</param>
        public int Execute(IConsoleOutput output)
        {
            Reset();

            const int STACK_LIMIT = int.MaxValue;
            int stackOverflow = 0;
            int @out = 0;

            //Create top level scope
            Scope.Push(new ScopeLoopPoint(false, new ScriptTargetWrapper(output, this)));

            while (Step(output, out int errOut))
            {
                stackOverflow++;

                if (errOut != 0)
                {
                    string descName = typeof(ErrorCode).GetFields().Where(i => (int)i.GetValue(null) == errOut).Select(i => i.Name).FirstOrDefault();

                    output.WriteError("====================================================");
                    output.WriteError("Script exited with bad error code!");
                    output.WriteError("* Error code:   " + errOut.ToString("x3") + "    " + descName);
                    output.WriteError("* Line:         " + (Index + 1).ToString("000") + "    " + Lines[Index]);
                    output.WriteError("Previous output messages might have more information");
                    output.WriteError("====================================================");
                    @out = errOut;
                    break;
                }
                else if (stackOverflow > STACK_LIMIT)
                {
                    output.WriteError("Infinite loop detected in script, aborting!!!");
                    @out = ErrorCode.INTERNAL_ERROR;
                    break;
                }
            }

            Command.AllCommandsInternal.RemoveAll(i => i is StandardLib.Structure.Call);

            return @out;
        }

        /// <summary>
        /// Resets the index of the script
        /// </summary>
        public void Reset()
        {
            Index = -1;
        }

        /// <summary>
        /// Creates a ScripHost instance from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ScriptHost FromFile(string path)
        {
            return new ScriptHost(System.IO.File.ReadAllLines(path));
        }

        /// <summary>
        /// Creates a ScriptHost instance from raw string data, one command per line
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ScriptHost FromRaw(string[] data)
        {
            return new ScriptHost(data);
        }

        internal void SendSkipMessage(string target, int count)
        {
            int targetAddress = Index;

            if (target != null)
            {
                while (true)
                {
                    if (Lines[targetAddress].Trim() == "label " + target.Trim())
                    {
                        Index = targetAddress;
                        return;
                    }

                    targetAddress++;
                    if (targetAddress >= Lines.Count)
                    {
                        targetAddress = 0;
                    }
                    if (targetAddress == Index)
                    {
                        Command.ThrowArgumentError("No label found with name '" + target + "'", ErrorCode.ARGUMENT_UNLISTED);
                    }
                }
            }
            else
            {
                int currentCount = 0;

                if (count == 0)
                {
                    Command.ThrowGenericError("Cannot skip 0 labels", ErrorCode.NUMBER_IS_ZERO);
                }
                else if (count > 0)
                {
                    while (true)
                    {
                        if (Lines[targetAddress].Trim().StartsWith("label") || Lines[targetAddress].Trim() == "label")
                        {
                            currentCount++;
                            if (currentCount == count)
                            {
                                Index = targetAddress;
                                return;
                            }
                        }

                        targetAddress++;
                        if (targetAddress >= Lines.Count)
                        {
                            Index = Lines.Count - 1;
                            return;
                        }
                    }
                }
                else if (count < 1)
                {
                    if (Lines[targetAddress].Trim().StartsWith("label "))
                    {
                        currentCount--;
                        if (currentCount == count)
                        {
                            Index = targetAddress;
                            return;
                        }
                    }

                    targetAddress--;
                    if (targetAddress < 0)
                    {
                        Index = 0;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Stops a script
        /// </summary>
        public void Abort()
        {
            Index = Lines.Count;
        }
    }

    internal class ScopeLoopPoint
    {
        public int ReturnPoint { get; set; }
        public bool HasReturnPoint { get; set; }
        public bool IgnoreLines { get; set; }

        public IEnumerator<string> Enumerator { get; set; }

        public VariableCollection Locals { get; }

        public ScopeLoopPoint(bool ignoreLines, ScriptTargetWrapper target)
        {
            IgnoreLines = ignoreLines;
            HasReturnPoint = false;
            ReturnPoint = -1;
            Enumerator = null;

            if (target.Session.Scope.Any())
            {
                Locals = new VariableCollection(target.Session.Scope.Peek().Locals);
            }
            else
            {
                Locals = VariableCollection.GlobalVariables;
            }
            
        }

        public ScopeLoopPoint(bool ignoreLines, int loopPoint, ScriptTargetWrapper target)
        {
            IgnoreLines = ignoreLines;
            HasReturnPoint = true;
            ReturnPoint = loopPoint;
            Enumerator = null;

            if (target.Session.Scope.Any())
            {
                Locals = new VariableCollection(target.Session.Scope.Peek().Locals);
            }
            else
            {
                Locals = VariableCollection.GlobalVariables;
            }
        }

        public ScopeLoopPoint(IEnumerator<string> enumerator, int loopPoint, ScriptTargetWrapper target)
        {
            IgnoreLines = false;
            HasReturnPoint = true;
            ReturnPoint = loopPoint;
            Enumerator = enumerator;

            if (target.Session.Scope.Any())
            {
                Locals = new VariableCollection(target.Session.Scope.Peek().Locals);
            }
            else
            {
                Locals = VariableCollection.GlobalVariables;
            }
        }
    }

    internal struct FunctionInfo
    {
        public string Name { get; set; }
        public int Line { get; set; }

        public FunctionInfo(string name, int line)
        {
            Name = name;
            Line = line;
        }
    }
}