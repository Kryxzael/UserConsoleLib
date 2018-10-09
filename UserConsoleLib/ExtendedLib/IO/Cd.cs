using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;


namespace UserConsoleLib.ExtendedLib.IO
{
    class Cd : Command
    {
        public override string Name => "cd";

        public override string HelpDescription => "Changes the active directory of the underlaying process";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("Path").Or();
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (args.Count == 0)
            {
                target.WriteLine(System.IO.Directory.GetCurrentDirectory());
                return;
            }
        
            string path = args.JoinEnd(0);

            try
            {
                System.IO.Directory.SetCurrentDirectory(path);
            }
            catch (PathTooLongException)
            {
                ThrowGenericError("The path was too long to parse", ErrorCode.GENERIC_1);
            }
            catch (DirectoryNotFoundException)
            {
                ThrowGenericError("Target directory was not found", ErrorCode.FILE_NOT_FOUND);
            }
            catch (FileNotFoundException)
            {
                ThrowGenericError("Target file was not found", ErrorCode.FILE_NOT_FOUND);
            }
            catch (IOException)
            {
                ThrowGenericError("An I/O error occured", ErrorCode.IO_ERROR);
            }
            catch (ArgumentNullException)
            {
                ThrowArgumentError(path, ErrorCode.ARGUMENT_NULL);
            }
            catch (ArgumentException)
            {
                ThrowGenericError("Parts of the path was invalid", ErrorCode.ARGUMENT_INVALID);
            }
            catch (SecurityException)
            {
                ThrowGenericError(path, ErrorCode.FILE_ACCESS_DENIED);
            }

            target.WriteLine(System.IO.Directory.GetCurrentDirectory());
        }
    }
}
