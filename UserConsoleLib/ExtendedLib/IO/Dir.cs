using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace UserConsoleLib.ExtendedLib.IO
{
    class Dir : Command
    {
        public override string Name => "dir";

        public override string HelpDescription => "Lists the contents of the current directory";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin()
                .Or()
                .Add("", "toarray");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            if (args.Count > 0)
            {
                target.WriteLine(string.Join(" ", new DirectoryInfo(Environment.CurrentDirectory).GetFileSystemInfos().Select(i => i.FullName).ToArray()));
            }
            else
            {
                target.WriteLine("Listing contents of: " + Environment.CurrentDirectory);

                foreach (FileSystemInfo i in new DirectoryInfo(Environment.CurrentDirectory).GetFileSystemInfos())
                {
                    if (i is DirectoryInfo)
                    {
                        target.WriteLine("* " + i.Name + "\\");
                    }
                    else
                    {
                        target.WriteLine("* " + i.Name);
                    }

                }
            }
            
        }
    }
}
