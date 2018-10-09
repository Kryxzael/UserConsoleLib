using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    /// <summary>
    /// Contains a reference guide for error codes returned by commands
    /// </summary>
    public static class ErrorCode
    {
        /// <summary>
        /// Indicates that the command was sucessfuly executed
        /// </summary>
        public const int SUCCESS = 0;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_1 = 0x01;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_2 = 0x02;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_3 = 0x03;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_4 = 0x04;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_5 = 0x05;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_6 = 0x06;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_7 = 0x07;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_8 = 0x08;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_9 = 0x09;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_10 = 0x0A;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_11 = 0x0B;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_12 = 0x0C;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_13 = 0x0D;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_14 = 0x0E;

        /// <summary>
        /// Indicates a user defined errror
        /// </summary>
        public const int GENERIC_15 = 0x0F;

        /// <summary>
        /// Indicates that the user passed to few arguments to the command
        /// </summary>
        public const int NOT_ENOUGH_ARGUMENTS = 0x10;

        /// <summary>
        /// Indicates that the user passed to many arguments to the command
        /// </summary>
        public const int TOO_MANY_ARGUMENTS = 0x11;

        /// <summary>
        /// Indicates that the user did not pass any arguments to the command
        /// </summary>
        public const int NO_ARGUMENTS = 0x12;

        /// <summary>
        /// Indicates that the user passed arguments to the command, when the command ask for none
        /// </summary>
        public const int NO_ARGUMENTS_ALLOWED = 0x13;

        /// <summary>
        /// Indicates that the user passed an unexpected argument count
        /// </summary>
        public const int UNKNOWN_ARGUMENT_COUNT = 0x14;

        /// <summary>
        /// Indicates that a value was not a number
        /// </summary>
        public const int NOT_A_NUMBER = 0x20;

        /// <summary>
        /// Indicates that a number was outside the expected range of numbers
        /// </summary>
        public const int NUMBER_OUT_OF_RANGE = 0x21;

        /// <summary>
        /// Indicates that a number was larger than the expected value
        /// </summary>
        public const int NUMBER_TOO_BIG = 0x22;

        /// <summary>
        /// Indicates that a number was smaller than the expected value
        /// </summary>
        public const int NUMBER_TOO_SMALL = 0x23;

        /// <summary>
        /// Indicates that a number was not an integer
        /// </summary>
        public const int NUMBER_NOT_INTEGER = 0x24;

        /// <summary>
        /// Indicates that a number was NegativeInfinity, PositiveInfinity or NaN
        /// </summary>
        public const int NUMBER_NOT_FINITE = 0x25;

        /// <summary>
        /// Indicates that a number was zero
        /// </summary>
        public const int NUMBER_IS_ZERO = 0x26;

        /// <summary>
        /// Indicates that a number was negative
        /// </summary>
        public const int NUMBER_IS_NEGATIVE = 0x27;

        /// <summary>
        /// Indicates that a number was positive
        /// </summary>
        public const int NUMBER_IS_POSITIVE = 0x28;

        /// <summary>
        /// Indicates that an argument did not correspond with any valid options for that argument
        /// </summary>
        public const int ARGUMENT_UNLISTED = 0x30;

        /// <summary>
        /// Indicates that an argument was invalid
        /// </summary>
        public const int ARGUMENT_INVALID = 0x31;

        /// <summary>
        /// Indicates that an argument was empty or null
        /// </summary>
        public const int ARGUMENT_NULL = 0x32;

        /// <summary>
        /// Indicates that an argument was empty or null
        /// </summary>
        public const int NOT_BOOLEAN = 0x33;

        /// <summary>
        /// Indicates that an argument contained invalid symbols 
        /// </summary>
        public const int ARGUMENT_NO_SYMBOL_ALLOWED = 0x34;

        /// <summary>
        /// Indicates that an argument contained digits
        /// </summary>
        public const int ARGUMENT_NO_NUMBER_ALLOWED = 0x35;

        /// <summary>
        /// Indicates that an argument contained spaces or underscores
        /// </summary>
        public const int ARGUMENT_NO_SPACES_ALLOWED = 0x36;

        /// <summary>
        /// Indicates that a list argument did not contain any valid values for the command to use
        /// </summary>
        public const int NO_VALID_VALUES = 0x37;

        /// <summary>
        /// Indicates that an operation failed because of the current context
        /// </summary>
        public const int INVALID_CONTEXT = 0x40;

        /// <summary>
        /// Indicates that the command attempted to do something invalid
        /// </summary>
        public const int INVALID_OPERATION = 0x41;

        /// <summary>
        /// Indicates that a command cannot be executed more that once
        /// </summary>
        public const int NON_REPEATABLE = 0x42;

        /// <summary>
        /// Indicates that the executor did not have proper access to the command 
        /// </summary>
        public const int UNAUTHORIZED = 0x43;

        /// <summary>
        /// Indicates that an internal error caused the command to fail
        /// </summary>
        public const int INTERNAL_ERROR = 0x44;

        /// <summary>
        /// Indicates that a command is unfinished
        /// </summary>
        public const int NOT_YET_IMPLEMENTED = 0x45;

        /// <summary>
        /// Indicates that the referenced file was not found
        /// </summary>
        public const int FILE_NOT_FOUND = 0x50;

        /// <summary>
        /// Indicates that the referenced folder, or part of a file path, was not found
        /// </summary>
        public const int DIRECTORY_NOT_FOUND = 0x51;

        /// <summary>
        /// Indicates that access to a file or folder was denied
        /// </summary>
        public const int FILE_ACCESS_DENIED = 0x52;

        /// <summary>
        /// Indicates that a file is read-only
        /// </summary>
        public const int FILE_LOCKED = 0x53;

        /// <summary>
        /// Indicates that a generic file streaming error occured
        /// </summary>
        public const int IO_ERROR = 0x54;
    }
}
