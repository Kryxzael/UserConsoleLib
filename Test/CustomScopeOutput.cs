using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserConsoleLib;
using UserConsoleLib.Scripting;

namespace Test
{
    public class CustomScopeOutput : IConsoleOutput
    {
        Scope s = new Scope();

        public void ClearBuffer()
        {
            Console.Clear();
        }

        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void ReadLine()
        {
            s.ParseLine(new Line(Console.ReadLine()), this);
        }
    }
}
