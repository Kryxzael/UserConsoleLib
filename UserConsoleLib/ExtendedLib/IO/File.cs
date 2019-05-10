using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.ExtendedLib.IO
{
    class File : Command
    {
        public override string Name => "file";

        public override string HelpDescription => "Tool for modifying files";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("option", "create", "delete", "read", "exists", "open").AddTrailing("path").Or()
                .Add("option", "move", "copy").Add("source").AddTrailing("destination").Or()
                .Add("option", "rename").Add("path").AddTrailing("newname").Or()
                .Add("option", "write").Add("path").AddTrailing("text");
        }

        protected override void Executed(Params args, IConsoleOutput target, Scope scope)
        {
            try
            {
                switch (args[0])
                {
                    case "create":
                        System.IO.File.Create(args.JoinEnd(1)).Close();
                        break;
                    case "delete":
                        System.IO.File.Delete(args.JoinEnd(1));
                        break;
                    case "read":
                        target.WriteLine(System.IO.File.ReadAllText(args.JoinEnd(1)));
                        break;
                    case "move":
                        System.IO.File.Move(args[1], args.JoinEnd(2));
                        break;
                    case "copy":
                        System.IO.File.Copy(args[1], args.JoinEnd(2));
                        break;
                    case "rename":
                        System.IO.File.Move(args[1], new System.IO.FileInfo(args[1]).Directory.FullName + "\\" + args.JoinEnd(2));
                        break;
                    case "write":
                        System.IO.File.WriteAllText(args[1], args.JoinEnd(2));
                        break;
                    case "exists":
                        target.WriteLine(System.IO.File.Exists(args.JoinEnd(1)));
                        break;
                    case "open":
                        System.Diagnostics.Process.Start(args.JoinEnd(1));
                        break;

                }
            }
            catch (UnauthorizedAccessException)
            {
                ThrowGenericError("Access denied", ErrorCode.FILE_ACCESS_DENIED);
            }
            catch (ArgumentNullException)
            {
                ThrowGenericError("Path was empty", ErrorCode.ARGUMENT_NULL);
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
                ThrowGenericError("Unexpected error", ErrorCode.IO_ERROR);
            }
            catch (NotSupportedException)
            {
                ThrowGenericError("Unexpected error", ErrorCode.IO_ERROR);
            }
            catch (ArgumentException)
            {
                ThrowGenericError("Path was invalid", ErrorCode.ARGUMENT_INVALID);
            }
            catch (System.Security.SecurityException)
            {
                ThrowGenericError("Access denied", ErrorCode.FILE_ACCESS_DENIED);
            }
            catch (Exception)
            {
                ThrowGenericError("Path was invalid", ErrorCode.IO_ERROR);
            }

        }
    }
}
