using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserConsoleLib;
using UserConsoleLib.Scripting;

namespace Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            ConsoleOutputRouter router = new ConsoleOutputRouter();
            ConsoleInterface @interface = new ConsoleInterface();

            router.AddListener(@interface, RouterOptions.ReceivesAll, new string[0]);
            router.AddListener(new FileInterface("C:\\users\\kryxzael\\desktop\\out.txt", true), RouterOptions.ReceivesAll, new string[0]);
            while (true)
            {
                @interface.ReadLine(router);
            }
        }
    }
}
