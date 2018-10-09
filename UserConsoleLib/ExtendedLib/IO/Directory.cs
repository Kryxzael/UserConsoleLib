using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib.ExtendedLib.IO
{
    class Directory : Command
    {
        public override string Name => "directory";

        public override string HelpDescription => "Tool for modifying files";

        public override Syntax GetSyntax(Params args)
        {
            return Syntax.Begin().Add("option", "create", "delete", "exists", "count", "strongdelete").AddTrailing("path").Or()
                .Add("option", "move", "copy").Add("source").AddTrailing("destination").Or()
                .Add("option", "rename").Add("path").AddTrailing("newname");
        }

        protected override void Executed(Params args, IConsoleOutput target)
        {
            try
            {
                switch (args[0])
                {
                    case "create":
                        System.IO.Directory.CreateDirectory(args.JoinEnd(1));
                        break;
                    case "delete":
                        if (System.IO.Directory.GetFileSystemEntries(args.JoinEnd(1)).Any())
                        {
                            ThrowGenericError("Directory was not empty. Use 'strongdelete' to override this restriction", ErrorCode.NUMBER_IS_POSITIVE);
                        }

                        System.IO.Directory.Delete(args.JoinEnd(1));
                        break;
                    case "strongdelete":
                        System.IO.Directory.Delete(args.JoinEnd(1), true);
                        break;
                    case "move":
                        System.IO.Directory.Move(args[1], args.JoinEnd(2));
                        break;
                    case "rename":
                        System.IO.Directory.Move(args[1], new System.IO.FileInfo(args[1]).Directory.FullName + "\\" + args.JoinEnd(2));
                        break;
                    case "exists":
                        target.WriteLine(System.IO.Directory.Exists(args.JoinEnd(1)));
                        break;
                    case "count":
                        target.WriteLine(System.IO.Directory.GetFileSystemEntries(args.JoinEnd(1)).Length);
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
            catch (CommandException)
            {
                throw;
            }
            catch (Exception)
            {
                ThrowGenericError("Path was invalid", ErrorCode.IO_ERROR);
            }

        }
    }
}
