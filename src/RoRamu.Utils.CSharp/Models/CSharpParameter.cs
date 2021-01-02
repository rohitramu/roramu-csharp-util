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
        public Type Type { get; }

        /// <summary>
        /// A description of this parameter as seen in documentation comments.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpParameter" />.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="description">A description of this parameter as seen in documentation comments.</param>
        public CSharpParameter(string name, Type type, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Description = description;
        }

        /// <summary>
        /// Converts this abstract representation of a C# parameter into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# parameter.</returns>
        public override string ToString()
        {
            return $"{this.Type.FullName} {this.Name}";
        }
    }
}
