using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserConsoleLib
{
    /// <summary>
    /// Represents a syntax list
    /// </summary>
    public class Syntax
    {
        static readonly string[] BOOLEAN_VALUES = new string[] { "yes", "no", "true", "false", "y", "n", "1", "0"  };
        static readonly string[] BOOLEAN_VALUES_STRICT = new string[] { "true", "false" };

        private Syntax() { }

        /// <summary>
        /// Gets the syntax that can be used as an alternative to this syntax
        /// </summary>
        public Syntax AlternateSyntax { get; private set; }

        /// <summary>
        /// If true this syntax will allow infinite trailing parameters
        /// </summary>
        public bool InfiniteTrailingArguments { get; private set; }

        /// <summary>
        /// Intenral list of items
        /// </summary>
        List<SyntaxItem> Items { get; } = new List<SyntaxItem>();

        /// <summary>
        /// Gets the count of arguments in this syntax
        /// </summary>
        public int ArgumentCount
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Returns true if this syntax has an alternate syntax
        /// </summary>
        public bool HasAlternateSyntax
        {
            get
            {
                return AlternateSyntax != null;
            }
        }

        /// <summary>
        /// Starts the definition of a syntax
        /// </summary>
        /// <returns></returns>
        public static Syntax Begin()
        {
            return new Syntax();
        }

        /// <summary>
        /// Adds a numeric parameter to this syntax
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="range">The range of valid values this parameter can have</param>
        /// <param name="intOnly">If true, this parameter must be an integer</param>
        /// <returns></returns>
        public Syntax Add(string name, Range range, bool intOnly)
        {
            Items.Add(new SyntaxItem() { Name = name, Range = range, Type = intOnly ? SyntaxItemType.Integer : SyntaxItemType.Number });
            return this;
        }

        /// <summary>
        /// Adds a parameter that can have one of many values
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="possibleValues">The values to chose from</param>
        /// <returns></returns>
        public Syntax Add(string name, params string[] possibleValues)
        {
            Items.Add(new SyntaxItem() { Name = name, ValidItems = possibleValues, Type = SyntaxItemType.List });
            return this;
        }

        /// <summary>
        /// Adds a parameter that can either be true or false
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="allowAnyBoolean">If true, the user can use values like 1/0, y/n or yes/no</param>
        /// <returns></returns>
        public Syntax Add(string name, bool allowAnyBoolean)
        {
            Items.Add(new SyntaxItem() { Name = name, ValidItems = !allowAnyBoolean ? BOOLEAN_VALUES_STRICT : BOOLEAN_VALUES, Type = SyntaxItemType.List });
            return this;
        }

        /// <summary>
        /// Adds a parameter with no restrictions
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <returns></returns>
        public Syntax Add(string name)
        {
            if (InfiniteTrailingArguments)
            {
                throw new InvalidOperationException("Cannot add more arguments after AddInfinite() has been used");
            }

            Items.Add(new SyntaxItem() { Name = name, Type = SyntaxItemType.Free });
            return this;
        }

        /// <summary>
        /// Adds a final parameter that can have spaces
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Syntax AddTrailing(string name)
        {
            Add(name);
            InfiniteTrailingArguments = true;
            return this;
        }

        /// <summary>
        /// Defines an alternate syntax
        /// </summary>
        /// <returns></returns>
        public Syntax Or()
        {
            Syntax s = Begin();
            s.AlternateSyntax = this;
            return s;
        }

        /// <summary>
        /// Syntax checks a list of arguments
        /// </summary>
        /// <param name="args">Arguments to check</param>
        /// <returns></returns>
        public bool IsCorrectSyntax(Params args)
        {
            return IsCorrectSyntax(null, args, false);
        }

        /// <summary>
        /// Syntax checks a list of arguments
        /// </summary>
        /// <param name="cmd">Command to pass to SyntaxErrors if any are present, not required if throwIf is set to false</param>
        /// <param name="args">Arguments to check</param>
        /// <param name="throwIf">If true: Throws an exception if syntax errors are found (generally passed to the IConsoleOuput target)</param>
        /// <returns></returns>
        internal bool IsCorrectSyntax(Command cmd, Params args, bool throwIf)
        {
            if (args.Count < Items.Count())
            {
                bool b = (AlternateSyntax?.IsCorrectSyntax(args)).GetValueOrDefault(false);

                if (!b && throwIf)
                {
                    Command.ThrowSyntaxError(cmd, args, ErrorCode.NOT_ENOUGH_ARGUMENTS);
                }

                return b;
            }

            if (!InfiniteTrailingArguments)
            {
                if (args.Count > Items.Count())
                {
                    bool b = (AlternateSyntax?.IsCorrectSyntax(args)).GetValueOrDefault(false);

                    if (!b && throwIf)
                    {
                        Command.ThrowSyntaxError(cmd, args, ErrorCode.TOO_MANY_ARGUMENTS);
                    }

                    return b;
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                switch (Items[i].Type)
                {
                    case SyntaxItemType.Free:
                        continue;
                    case SyntaxItemType.Number:
                        if (args.IsDouble(i) && Items[i].Range.IsInRange(args.ToDouble(i)))
                        {
                            continue;
                        }

                        bool b = (AlternateSyntax?.IsCorrectSyntax(args)).GetValueOrDefault(false);
                        if (!b && throwIf)
                        {
                            if (!args.IsDouble(i))
                            {
                                Command.ThrowNaNError(args[i], ErrorCode.NOT_A_NUMBER);
                            }
                            else
                            {
                                Command.ThrowOutOfRangeError(args.ToDouble(i), Items[i].Range, ErrorCode.NUMBER_OUT_OF_RANGE);
                            }
                        }

                        return b;
                    case SyntaxItemType.Integer:
                        if (args.IsInteger(i) && Items[i].Range.IsInRange(args.ToInt(i)))
                        {
                            continue;
                        }

                        bool b2 = (AlternateSyntax?.IsCorrectSyntax(args)).GetValueOrDefault(false);
                        if (!b2 && throwIf)
                        {
                            if (!args.IsInteger(i))
                            {
                                if (args.IsDouble(i))
                                {
                                    Command.ThrowNoFloatsAllowedError(args.ToDouble(i), ErrorCode.NUMBER_NOT_INTEGER);
                                }
                                else
                                {
                                    Command.ThrowNaNError(args[i], ErrorCode.NOT_A_NUMBER);
                                }
                                
                            }
                            else
                            {
                                Command.ThrowOutOfRangeError(args.ToDouble(i), Items[i].Range, ErrorCode.NUMBER_OUT_OF_RANGE);
                            }
                        }

                        return b2;

                    case SyntaxItemType.List:
                        if (Items[i].ValidItems.Contains(args[i]))
                        {
                            continue;
                        }
                        bool b3 = (AlternateSyntax?.IsCorrectSyntax(args)).GetValueOrDefault(false);

                        if (!b3 && throwIf)
                        {
                            Command.ThrowArgumentError(args[i], Items[i].ValidItems, ErrorCode.ARGUMENT_UNLISTED);
                        }

                        return b3;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a syntax string of this syntax and any alternate syntaxes
        /// </summary>
        /// <returns></returns>
        public string CreateSyntaxString()
        {
            string _ = "";

            if (HasAlternateSyntax)
            {
                _ += AlternateSyntax.CreateSyntaxString() + "\n";
            }

            if (!Items.Any())
            {
                _ += "(none) ";
            }

            foreach (SyntaxItem i in Items)
            {
                _ += "<";

                _ += i.Name;

                switch (i.Type)
                {
                    case SyntaxItemType.Free:
                        break;
                    case SyntaxItemType.Number:
                    case SyntaxItemType.Integer:
                        _ += i.Range.ToString();
                        break;
                    case SyntaxItemType.List:
                        if (i.ValidItems == BOOLEAN_VALUES || i.ValidItems == BOOLEAN_VALUES_STRICT)
                        {
                            _ += " (true|false)";
                        }
                        else
                        {
                            _ += " (" + string.Join("|", i.ValidItems) + ")";
                        }
                        
                        break;
                }

                _ += "> ";
            }

            if (InfiniteTrailingArguments)
            {
                _ += "... ";
            }

            return _.TrimStart();
        }

        /// <summary>
        /// Gets the identifier of an argument
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetArgumentName(int index)
        {
            return Items[index].Name;
        }

        /// <summary>
        /// Gets the minimum value of a numeric argument
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetArgumentMinValue(int index)
        {
            if (!ArgumentIsNumber(index))
            {
                throw new ArgumentException("The specified argument is not a numeric argument");
            }

            return Items[index].Range.Minimum;
        }

        /// <summary>
        /// Gets the maximum value of a numeric argument
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetArgumentMaxValue(int index)
        {
            if (!ArgumentIsNumber(index))
            {
                throw new ArgumentException("The specified argument is not a numeric argument");
            }

            return Items[index].Range.Maximum;
        }

        /// <summary>
        /// Gets the list of valid arguments of a multiple-choice argument
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<string> GetArgumentValidItems(int index)
        {
            if (!ArgumentIsChoice(index))
            {
                throw new ArgumentException("The specified argument is not a multiple-choice argument");
            }

            return Items[index].ValidItems.AsEnumerable();
        }

        /// <summary>
        /// Returns true if an argument accepts numbers
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ArgumentIsNumber(int index)
        {
            return Items[index].Type == SyntaxItemType.Number || Items[index].Type == SyntaxItemType.Integer;
        }

        /// <summary>
        /// Returns true if an argument is limited to a set of possible values
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ArgumentIsChoice(int index)
        {
            return Items[index].Type == SyntaxItemType.List;
        }

        /// <summary>
        /// Returns true if an argument accepts integers only
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ArgumentIsInteger(int index)
        {
            return Items[index].Type == SyntaxItemType.Integer;
        }

        /// <summary>
        /// Returns true if an argument accepts any value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ArgumentIsAny(int index)
        {
            return Items[index].Type == SyntaxItemType.Free;
        }

        /// <summary>
        /// Creates a syntax string of this syntax and any alternate syntaxes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Syntax: " + CreateSyntaxString();
        }

        /// <summary>
        /// Checks whether two syntaxes are identical
        /// </summary>
        /// <param name="left">Left syntax</param>
        /// <param name="right">Right syntax</param>
        /// <returns></returns>
        public static bool operator ==(Syntax left, Syntax right)
        {
            if ((object)left == null)
            {
                if ((object)right == null)
                {
                    return true;
                }
                return false;
            }
            else if ((object)right == null)
            {
                return false;
            }

            if (left.Items.SequenceEqual(right.Items) && left.InfiniteTrailingArguments == right.InfiniteTrailingArguments)
            {
                if (left.HasAlternateSyntax)
                {
                    if (right.HasAlternateSyntax)
                    {
                        return left.AlternateSyntax == right.AlternateSyntax;
                    }

                    return false;
                }
                else if (right.HasAlternateSyntax)
                {
                    return false;
                }
            }

            return left.Items.SequenceEqual(right.Items);
        }

        /// <summary>
        /// Checks whether two syntaxes differ 
        /// </summary>
        /// <param name="left">Left syntax</param>
        /// <param name="right">Right syntax</param>
        /// <returns></returns>
        public static bool operator !=(Syntax left, Syntax right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Checks whether this syntax is equal to another object
        /// </summary>
        /// <param name="obj">Object to test, if anything other than a Syntax object: this function will return false</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Syntax i)
            {
                return this == i;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this Syntax
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (HasAlternateSyntax)
            {
                return Items.GetHashCode() ^ AlternateSyntax.GetHashCode() ^ InfiniteTrailingArguments.GetHashCode();
            }

            return Items.GetHashCode() ^ InfiniteTrailingArguments.GetHashCode();
        }


        /// <summary>
        /// Internal serialization of a syntax entry
        /// </summary>
        private class SyntaxItem
        {
            public string Name { get; set; }
            public SyntaxItemType Type { get; set; }
            public string[] ValidItems { get; set; }
            public Range Range { get; set; }
        }

        /// <summary>
        /// Internal representation of a syntax entry type
        /// </summary>
        private enum SyntaxItemType
        {
            Free = 0,
            Number = 1,
            Integer = 2,
            List = 4
        }
    }
}
