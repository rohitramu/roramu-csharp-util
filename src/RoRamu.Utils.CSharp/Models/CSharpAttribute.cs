namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a C# attribute.
    /// </summary>
    public class CSharpAttribute
    {
        /// <summary>
        /// The name of the attribute.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The attribute's parameters.
        /// </summary>
        public IEnumerable<string> Parameters { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpAttribute" /> object.
        /// </summary>
        /// <param name="name">The name of the attribute (i.e. the attribute's type).</param>
        /// <param name="arguments">The attribute's arguments as strings.  These should be passed in as they would appear in code.</param>
        public CSharpAttribute(string name, params string[] arguments) : this(name, arguments.AsEnumerable()) { }

        /// <summary>
        /// Creates a new <see cref="CSharpAttribute" /> object.
        /// </summary>
        /// <param name="name">The name of the attribute (i.e. the attribute's type).</param>
        /// <param name="arguments">The attribute's arguments as strings.  These should be the strings as they would appear in code.</param>
        public CSharpAttribute(string name, IEnumerable<string> arguments)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("C# Property name cannot be null or whitespace", nameof(name));
            }

            this.Name = name;
            this.Parameters = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Converts this abstract representation of a C# attribute into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# attribute.</returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Converts this abstract representation of a C# attribute into a string representation (i.e. code).
        /// </summary>
        /// <param name="multiLineArguments">Whether or not to put each of the attribute's parameters on a separate line.</param>
        /// <returns>The string representation of this C# attribute.</returns>
        public string ToString(bool multiLineArguments = false)
        {
            string attributeName = this.Name.EndsWith("Attribute")
                ? this.Name.Substring(0, this.Name.LastIndexOf("Attribute"))
                : this.Name;

            string argumentString;
            if (this.Parameters.Any())
            {
                if (multiLineArguments)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine();
                    string lastArgument = this.Parameters.Last();
                    foreach (string argument in this.Parameters)
                    {
                        if (argument != lastArgument)
                        {
                            stringBuilder.AppendLine($"{argument},".Indent());
                        }
                        else
                        {
                            stringBuilder.AppendLine(argument.Indent());
                        }
                    }

                    argumentString = $"({stringBuilder})";
                }
                else
                {
                    argumentString = $"({string.Join(", ", this.Parameters)})";
                }
            }
            else
            {
                argumentString = string.Empty;
            }

            return $"[{attributeName}{argumentString}]";
        }
    }
}
