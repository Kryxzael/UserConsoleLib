using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib.StandardLib.Control
{
    class Load : Command
    {
        public override string Name => "load";

        public override string HelpDescription => "Reads and executes a script file";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().AddTrailing("Path");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            ScriptHost host = null;

            try
            {
                host = ScriptHost.FromFile(args.JoinEnd(0));
            }
            catch (ArgumentNullException)
            {
                ThrowGenericError("Path was empty", ErrorCode.ARGUMENT_NULL);
            }
            catch (ArgumentException)
            {
                ThrowArgumentError("Path was invalid", ErrorCode.ARGUMENT_INVALID);
            }
            catch (System.IO.FileNotFoundException)
            {
                ThrowGenericError("File not found", ErrorCode.FILE_NOT_FOUND);
            }
            catch (System.IO.PathTooLongException)
            {
                ThrowGenericError("Path was too long", ErrorCode.ARGUMENT_INVALID);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                ThrowGenericError("Parts of the specified path was not found", ErrorCode.DIRECTORY_NOT_FOUND);
            }
            catch (System.IO.IOException)
            {
                ThrowGenericError("Unexpected error opening script", ErrorCode.IO_ERROR);
            }
            catch (UnauthorizedAccessException)
            {
                ThrowGenericError("Access denied", ErrorCode.FILE_ACCESS_DENIED);
            }
            catch (NotSupportedException)
            {
                ThrowGenericError("Unexpected error opening script", ErrorCode.IO_ERROR);
            }
            catch (System.Security.SecurityException)
            {
                ThrowGenericError("Access denied", ErrorCode.FILE_ACCESS_DENIED);
            }

            host.Execute(target);
        }
    }
}
