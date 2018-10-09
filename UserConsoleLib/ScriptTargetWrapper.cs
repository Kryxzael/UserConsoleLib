using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    class ScriptTargetWrapper : IConsoleOutput
    {
        IConsoleOutput Internal { get; }
        public ScriptHost Session { get; }

        public ScriptTargetWrapper(IConsoleOutput output, ScriptHost session)
        {
            Internal = output;
            Session = session;
        }

        public void ClearBuffer()
        {
            Internal.ClearBuffer();
        }

        public void WriteError(string message)
        {
            Internal.WriteError(message);
        }

        public void WriteLine(string message)
        {
            Internal.WriteLine(message);
        }

        public void WriteWarning(string message)
        {
            Internal.WriteWarning(message);
        }
    }
}
