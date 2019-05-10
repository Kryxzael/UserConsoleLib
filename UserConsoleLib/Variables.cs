using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace UserConsoleLib
{
    /// <summary>
    /// Support for local variable implementation
    /// </summary>
    public class VariableCollection
    {
        /// <summary>
        /// The collection of variables parented to this collection
        /// </summary>
        public VariableCollection ParentCollection { get; }

        /// <summary>
        /// Creates a new variable collection with a link to the parent collection
        /// </summary>
        /// <param name="parent">Parent collection</param>
        public VariableCollection(VariableCollection parent)
        {
            ParentCollection = parent;
        }

        /// <summary>
        /// Creates a new variable collection
        /// </summary>
        public VariableCollection()
        {

        }

        private Dictionary<string, string> Vars { get; } = new Dictionary<string, string>()
        {
            { "dev", "..\\..\\TestScript.txt" }
        };
        
        /// <summary>
        /// Gets a read-only representation of all registered variables
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>> AllVariables => Vars.ToList().AsReadOnly();

        /// <summary>
        /// Returns true if a variable is defined with a specific name
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <returns></returns>
        public bool IsDefined(string name)
        {
            return Vars.ContainsKey(name) || (ParentCollection?.IsDefined(name)).GetValueOrDefault();
        }

        /// <summary>
        /// Assigns a value to a variable
        /// </summary>
        /// <param name="name">Name of variable to set</param>
        /// <param name="value">Value to assign</param>
        public void Set(string name, string value)
        {
            if (!IsDefined(name) || Vars.ContainsKey(name))
            {
                Vars[name] = value;
                return;
            }

            ParentCollection.Set(name, value);
        }

        /// <summary>
        /// Assigns a value to a variable
        /// </summary>
        /// <param name="name">Name of variable to set</param>
        /// <param name="value">Value to assign</param>
        public void Set(string name, double value)
        {
            Set(name, ConConverter.ToString(value));
        }

        /// <summary>
        /// Assigns a value to a variable
        /// </summary>
        /// <param name="name">Name of variable to set</param>
        /// <param name="value">Value to assign</param>
        public void Set(string name, int value)
        {
            Set(name, ConConverter.ToString(value));
        }

        /// <summary>
        /// Assigns a value to a variable
        /// </summary>
        /// <param name="name">Name of variable to set</param>
        /// <param name="value">Value to assign</param>
        public void Set(string name, bool value)
        {
            Set(name, ConConverter.ToString(value));
        }

        /// <summary>
        /// Gets the value of a variable
        /// </summary>
        /// <param name="name">Name of variable to get</param>
        /// <returns></returns>
        public string Get(string name)
        {
            if (IsDefined(name))
            {
                return Vars[name];
            }

            if (ParentCollection != null)
            {
                return ParentCollection.Get(name);
            }

            throw new ArgumentException("No variable with this name is defined");
        }

        /// <summary>
        /// Gets the value of a variable or null if it does not exist
        /// </summary>
        /// <param name="name">Name of variable to get</param>
        /// <returns></returns>
        public string GetOrDefault(string name)
        {
            return GetOrDefault(name, null);
        }

        /// <summary>
        /// Gets the value of a variable or another value if it does not exist
        /// </summary>
        /// <param name="name">Name of variable to get</param>
        /// <param name="defaultValue">Value to return if the variable is undefined</param>
        /// <returns></returns>
        public string GetOrDefault(string name, string defaultValue)
        {
            if (IsDefined(name))
            {
                return Vars[name];
            }

            if (ParentCollection != null)
            {
                return ParentCollection.GetOrDefault(name, defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// Removes the definition of a variable
        /// </summary>
        /// <param name="name">Name of variable to remove</param>
        /// <returns></returns>
        public bool Undefine(string name)
        {
            return Vars.Remove(name);
        }
    }
}
