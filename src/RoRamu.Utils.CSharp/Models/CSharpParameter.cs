namespace RoRamu.Utils.CSharp
{
    using System;

    /// <summary>
    /// Represents a C# method parameter.
    /// </summary>
    public class CSharpParameter
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// A description of this parameter as seen in documentation comments.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpParameter" />.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="typeName">The parameter type's name.  Providing the full name is recommended.</param>
        /// <param name="description">A description of this parameter as seen in documentation comments.</param>
        public CSharpParameter(string name, string typeName, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            this.Description = description;
        }

        /// <summary>
        /// Converts this abstract representation of a C# parameter into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# parameter.</returns>
        public override string ToString()
        {
            return $"{this.TypeName} {CSharpNamingUtils.SanitizeIdentifier(this.Name)}";
        }
    }
}
