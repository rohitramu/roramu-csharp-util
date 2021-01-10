﻿namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a C# property.
    /// </summary>
    public class CSharpProperty
    {
        /// <summary>
        /// The name of this property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of this property.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Whether or not this is overriding a property in a base type.
        /// </summary>
        public bool IsOverride { get; }

        /// <summary>
        /// The access level of this property.
        /// </summary>
        public CSharpAccessModifier AccessModifier { get; }

        /// <summary>
        /// The attributes on this property.
        /// </summary>
        public IEnumerable<CSharpAttribute> Attributes { get; }

        /// <summary>
        /// The documentation comment for this property.
        /// </summary>
        public CSharpDocumentationComment DocumentationComment { get; }

        /// <summary>
        /// Constructs a new <see cref="CSharpProperty" /> object.
        /// </summary>
        /// <param name="name">The name of this property.</param>
        /// <param name="type">The type of this property.</param>
        /// <param name="isOverride">Whether or not this is overriding a property in a base type.</param>
        /// <param name="accessModifier">The access level of this property.</param>
        /// <param name="attributes">The attributes on this property.</param>
        /// <param name="documentationComment">The documentation comment for this property.</param>
        public CSharpProperty(string name, string type, bool isOverride, CSharpAccessModifier accessModifier, IEnumerable<CSharpAttribute> attributes, CSharpDocumentationComment documentationComment)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("C# Property name cannot be null or whitespace", nameof(name));
            }
            if (type != null && string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Return type cannot be empty or whitespace", nameof(name));
            }

            this.Name = name;
            this.TypeName = type ?? throw new ArgumentNullException(nameof(type));
            this.IsOverride = isOverride;
            this.AccessModifier = accessModifier;
            this.Attributes = attributes ?? Array.Empty<CSharpAttribute>();
            this.DocumentationComment = documentationComment;
        }

        /// <summary>
        /// Converts this abstract representation of a C# property into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# property.</returns>
        public override string ToString()
        {
            // Create a string builder
            StringBuilder resultBuilder = new StringBuilder();

            // Add the documentation comment
            string docComment = this.DocumentationComment?.ToString();
            if (!string.IsNullOrWhiteSpace(docComment))
            {
                resultBuilder.AppendLine(docComment);
            }

            // Loop through attributes
            foreach (CSharpAttribute attribute in this.Attributes)
            {
                resultBuilder.AppendLine(attribute.ToString());
            }

            // Add the property definition itself
            resultBuilder.Append($"{this.AccessModifier.ToCSharpString()}{(this.IsOverride ? " override" : string.Empty)} {this.TypeName} {CSharpNamingUtils.SanitizeIdentifier(this.Name)} {{ get; set; }}");

            // Compile the string
            string result = resultBuilder.ToString();

            return result;
        }
    }
}
