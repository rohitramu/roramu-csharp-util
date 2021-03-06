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
        /// Whether or not this is a static property.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// Whether or not this is overriding a property in a base type.
        /// </summary>
        public bool IsOverride { get; }

        /// <summary>
        /// The access level of this property.
        /// </summary>
        public CSharpAccessModifier AccessModifier { get; }

        /// <summary>
        /// Whether or not this property has a getter.
        /// </summary>
        public bool HasGetter { get; }

        /// <summary>
        /// Whether or not this property has a setter.
        /// </summary>
        public bool HasSetter { get; }

        /// <summary>
        /// The default value for this property.
        /// </summary>
        public string DefaultValue { get; }

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
        /// <param name="accessModifier">The access level of this property.</param>
        /// <param name="isStatic">Whether or not this is a static property.</param>
        /// <param name="isOverride">Whether or not this is overriding a property in a base type.</param>
        /// <param name="hasGetter">Whether or not this property has a getter.</param>
        /// <param name="hasSetter">Whether or not this property has a setter.</param>
        /// <param name="defaultValue">The default value for this property.</param>
        /// <param name="attributes">The attributes on this property.</param>
        /// <param name="documentationComment">The documentation comment for this property.</param>
        public CSharpProperty(
            string name,
            string type,
            CSharpAccessModifier accessModifier = CSharpAccessModifier.Public,
            bool isStatic = false,
            bool isOverride = false,
            bool hasGetter = true,
            bool hasSetter = true,
            string defaultValue = null,
            IEnumerable<CSharpAttribute> attributes = null,
            CSharpDocumentationComment documentationComment = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("C# Property name cannot be null or whitespace", nameof(name));
            }
            if (type != null && string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Property type cannot be empty or whitespace", nameof(name));
            }

            this.Name = name;
            this.TypeName = type ?? throw new ArgumentNullException(nameof(type));
            this.IsStatic = isStatic;
            this.IsOverride = isOverride;
            this.AccessModifier = accessModifier;
            this.HasGetter = hasGetter;
            this.HasSetter = hasSetter;
            this.DefaultValue = defaultValue;
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
            StringBuilder sb = new StringBuilder();

            // Add the documentation comment
            string docComment = this.DocumentationComment?.ToString();
            if (!string.IsNullOrWhiteSpace(docComment))
            {
                sb.AppendLine(docComment);
            }

            // Loop through attributes
            foreach (CSharpAttribute attribute in this.Attributes)
            {
                sb.AppendLine(attribute.ToString());
            }

            // Add the property definition itself
            sb.Append(this.AccessModifier.ToCSharpString());
            if (this.IsStatic)
            {
                sb.Append(" static");
            }
            if (this.IsOverride)
            {
                sb.Append(" override");
            }
            sb.Append($" {this.TypeName} {CSharpNamingUtils.SanitizeIdentifier(this.Name)}");
            sb.Append(" {");
            if (this.HasGetter)
            {
                sb.Append(" get;");
            }
            if (this.HasSetter)
            {
                sb.Append(" set;");
            }
            sb.Append(" }");

            // Add the default value
            if (!string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                sb.Append($" = {this.DefaultValue};");
            }

            // Compile the string
            string result = sb.ToString();

            return result;
        }
    }
}
