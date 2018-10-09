using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace UserConsoleLib
{
    /// <summary>
    /// Acts as a converter module for performing standarized conversion between console interface and objects
    /// </summary>
    public static class ConConverter
    {
        internal static readonly CultureInfo DefaultInfo = CultureInfo.GetCultureInfo("en-US");

        /// <summary>
        /// Converts a string to an int32
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static int? ToInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, DefaultInfo, out int i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an byte
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static byte? ToByte(string value)
        {
            if (byte.TryParse(value, NumberStyles.Integer, DefaultInfo, out byte i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an int16
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static short? ToShort(string value)
        {
            if (short.TryParse(value, NumberStyles.Integer, DefaultInfo, out short i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an int64
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static long? ToLong(string value)
        {
            if (long.TryParse(value, NumberStyles.Integer, DefaultInfo, out long i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a signed byte
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static sbyte? ToSByte(string value)
        {
            if (sbyte.TryParse(value, NumberStyles.Integer, DefaultInfo, out sbyte i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an unsigned int16
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static ushort? ToUShort(string value)
        {
            if (ushort.TryParse(value, NumberStyles.Integer, DefaultInfo, out ushort i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an unsigned int32
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static uint? ToUInt(string value)
        {
            if (uint.TryParse(value, NumberStyles.Integer, DefaultInfo, out uint i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to an unsigned int64
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static ulong? ToULong(string value)
        {
            if (ulong.TryParse(value, NumberStyles.Integer, DefaultInfo, out ulong i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a single
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static float? ToFloat(string value)
        {
            if (float.TryParse(value, NumberStyles.Float, DefaultInfo, out float i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a double
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static double? ToDouble(string value)
        {
            if (double.TryParse(value, NumberStyles.Float, DefaultInfo, out double i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a decimal
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static decimal? ToDecimal(string value)
        {
            if (decimal.TryParse(value, NumberStyles.Float, DefaultInfo, out decimal i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a DateTime instance
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(string value)
        {
            if (DateTime.TryParse(value, DefaultInfo, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out DateTime i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a TimeSpan instance
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpan(string value)
        {
            if (TimeSpan.TryParse(value, out TimeSpan i))
            {
                return i;
            }

            return null;
        }

        /// <summary>
        /// Converts a string to a boolean
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static bool? ToBool(string value)
        {
            switch (value)
            {
                case "true":
                case "yes":
                case "1":
                case "y":
                    return true;
                case "false":
                case "no":
                case "0":
                case "n":
                    return false;
            }

            return null;
        }

        /// <summary>
        /// Converts a byte to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(byte value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an int16 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(short value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an int32 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(int value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an int64 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(long value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a signed byte to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(sbyte value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an unsigned int16 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(ushort value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an unsigned int32 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(uint value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts an unsigned int64 to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(ulong value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a single to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(float value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a double to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(double value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a decimal to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(decimal value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a DateTime instance to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(DateTime value)
        {
            return value.ToString(DefaultInfo);
        }

        /// <summary>
        /// Converts a TimeSpan instance to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(TimeSpan value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts a boolean to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string ToString(bool value)
        {
            if (value)
            {
                return "true";
            }

            return "false";
        }

        /// <summary>
        /// Writes a number to the console output device
        /// </summary>
        /// <param name="target">Output device</param>
        /// <param name="value">Value to write</param>
        public static void WriteLine(this IConsoleOutput target, double value)
        {
            target.WriteLine(ToString(value));
        }

        /// <summary>
        /// Writes an integer to the console output device
        /// </summary>
        /// <param name="target">Output device</param>
        /// <param name="value">Value to write</param>
        public static void WriteLine(this IConsoleOutput target, int value)
        {
            target.WriteLine(ToString(value));
        }

        /// <summary>
        /// Writes a boolean to the console output device
        /// </summary>
        /// <param name="target">Output device</param>
        /// <param name="value">Value to write</param>
        public static void WriteLine(this IConsoleOutput target, bool value)
        {
            target.WriteLine(ToString(value));
        }

        /// <summary>
        /// Gets a list with the given string id
        /// </summary>
        /// <param name="idstr">ID of list enclosed in double square brackets</param>
        /// <returns></returns>
        public static List<string> GetList(string idstr)
        {
            //Decodes the ID of the list
            int? v = ConConverter.ToInt(idstr.TrimStart('[').TrimEnd(']'));

            //If the ID was invalid
            if (v == null)
            {
                Command.ThrowNaNError(idstr, ErrorCode.NOT_A_NUMBER);
            }

            //If the ID is unclaimed
            if (!StandardLib.Variables.List.Registry.ContainsKey(v.Value))
            {
                Command.ThrowGenericError("Attempted access of undefined list", ErrorCode.INVALID_CONTEXT);
            }

            return StandardLib.Variables.List.Registry[v.Value];
        }

        /// <summary>
        /// Gets a list by a numeric id
        /// </summary>
        /// <param name="id">numeric id of list</param>
        /// <returns></returns>
        public static List<string> GetList(int id)
        {
            return StandardLib.Variables.List.Registry[id];
        }
    }
}
