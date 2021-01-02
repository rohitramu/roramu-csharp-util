﻿namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a C# class.
    /// </summary>
    public class CSharpClass
    {
        /// <summary>
        /// The name of the class.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The class' access modifier.
        /// </summary>
        public CSharpAccessModifier AccessModifier { get; }

        /// <summary>
        /// The class' base (super/parent) type.
        /// </summary>
        public string BaseType { get; }

        /// <summary>
        /// The interfaces that this class implements.
        /// </summary>
        public IEnumerable<string> Interfaces { get; }

        /// <summary>
        /// The attributes on this class.
        /// </summary>
        public IEnumerable<CSharpAttribute> Attributes { get; }

        /// <summary>
        /// The properties in this class.
        /// </summary>
        public IEnumerable<CSharpProperty> Properties { get; }

        /// <summary>
        /// The methods in this class.
        /// </summary>
        public IEnumerable<CSharpMethod> Methods { get; }

        /// <summary>
        /// The documentation comment on this class.
        /// </summary>
        public CSharpDocumentationComment DocumentationComment { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpClass" /> object.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        /// <param name="accessModifier">The class' access modifier.</param>
        /// <param name="baseType">The class' base (super/parent) type.</param>
        /// <param name="interfaces">The interfaces that this class implements.</param>
        /// <param name="attributes">The attributes on this class.</param>
        /// <param name="properties">The properties in this class.</param>
        /// <param name="methods">The methods in this class.</param>
        /// <param name="documentationComment">The documentation comment on this class.</param>
        public CSharpClass(string name, CSharpAccessModifier accessModifier, string baseType, IEnumerable<string> interfaces, IEnumerable<CSharpAttribute> attributes, IEnumerable<CSharpProperty> properties, IEnumerable<CSharpMethod> methods, CSharpDocumentationComment documentationComment)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("C# Class name cannot be null or whitespace", nameof(name));
            }

            if (baseType != null && baseType.Trim() == string.Empty)
            {
                throw new ArgumentException("Base type name cannot be empty.  Set it to null to remove the base type.", nameof(baseType));
            }

            this.Name = name;
            this.AccessModifier = accessModifier;
            this.BaseType = baseType;
            this.Interfaces = interfaces == null
                ? Array.Empty<string>().AsEnumerable()
                : new HashSet<string>(interfaces);
            this.Attributes = attributes ?? Array.Empty<CSharpAttribute>();
            this.Properties = properties ?? Array.Empty<CSharpProperty>();
            this.Methods = methods ?? Array.Empty<CSharpMethod>();
            this.DocumentationComment = documentationComment;
        }

        /// <summary>
        /// Converts this abstract representation of a C# class into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this class.</returns>
        public override string ToString()
        {
            StringBuilder resultBuilder = new StringBuilder();

            // Documentation comment
            if (this.DocumentationComment != null)
            {
                resultBuilder.AppendLine(this.DocumentationComment.ToString());
            }

            // Attributes
            foreach (CSharpAttribute attribute in this.Attributes)
            {
                resultBuilder.AppendLine(attribute.ToString());
            }

            // Add the first line of the class definition (access modifiers, class name, inheritance)
            IEnumerable<string> parents = this.BaseType == null
                ? this.Interfaces
                : this.BaseType.SingleObjectAsEnumerable().Concat(this.Interfaces);
            string inheritance = parents.Any() ? $" : {string.Join(", ", parents)}" : string.Empty;
            string nameLine = $"{AccessModifier.ToCSharpString()} class {this.Name}{inheritance}";
            resultBuilder.AppendLine(nameLine);

            // Start body
            resultBuilder.AppendLine("{");

            // Properties
            bool isFirst = true;
            foreach (CSharpProperty property in this.Properties)
            {
                // Add a new line except for the first property
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    resultBuilder.AppendLine();
                }

                resultBuilder.AppendLine(property.ToString().Indent());
            }

            if (this.Properties.Any() && this.Methods.Any())
            {
                resultBuilder.AppendLine();
            }

            // Methods
            isFirst = true;
            foreach (CSharpMethod method in this.Methods)
            {
                // Add a new line except for the first property
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    resultBuilder.AppendLine();
                }

                resultBuilder.AppendLine(method.ToString().Indent());
            }

            // End body
            resultBuilder.AppendLine("}");

            // Compile and return result
            string result = resultBuilder.ToString().Trim();
            return result;
        }
    }
}