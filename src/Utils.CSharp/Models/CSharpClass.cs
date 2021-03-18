namespace RoRamu.Utils.CSharp
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
        /// True if the class is static, otherwise false.
        /// </summary>
        public bool IsStatic { get; }

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
        /// The constructors in this class.
        /// </summary>
        public IEnumerable<CSharpClassConstructor> Constructors { get; }

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
        /// <param name="constructors">The constructor methods for this class.</param>
        /// <param name="methods">The methods in this class.</param>
        /// <param name="isStatic">True if the class should be marked as static, otherwise false.</param>
        /// <param name="documentationComment">The documentation comment on this class.</param>
        public CSharpClass(
            string name,
            CSharpAccessModifier accessModifier = CSharpAccessModifier.Public,
            IEnumerable<CSharpProperty> properties = null,
            IEnumerable<CSharpClassConstructor> constructors = null,
            IEnumerable<CSharpMethod> methods = null,
            bool isStatic = false,
            string baseType = null,
            IEnumerable<string> interfaces = null,
            IEnumerable<CSharpAttribute> attributes = null,
            CSharpDocumentationComment documentationComment = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Class name cannot be null or whitespace", nameof(name));
            }

            if (baseType != null && string.IsNullOrWhiteSpace(baseType))
            {
                throw new ArgumentException("Base type name cannot be empty.  Set it to null to remove the base type.", nameof(baseType));
            }

            this.Name = name;
            this.AccessModifier = accessModifier;
            this.BaseType = baseType;
            this.IsStatic = isStatic;
            this.Interfaces = interfaces == null
                ? Array.Empty<string>().AsEnumerable()
                : new HashSet<string>(interfaces);
            this.Attributes = attributes ?? Array.Empty<CSharpAttribute>();
            this.Properties = properties ?? Array.Empty<CSharpProperty>();
            this.Constructors = constructors ?? Array.Empty<CSharpClassConstructor>();
            this.Methods = methods ?? Array.Empty<CSharpMethod>();
            this.DocumentationComment = documentationComment;
        }

        /// <summary>
        /// Converts this abstract representation of a C# class into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this class.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Documentation comment
            string docComment = this.DocumentationComment?.ToString();
            if (!string.IsNullOrWhiteSpace(docComment))
            {
                sb.AppendLine(docComment);
            }

            // Attributes
            foreach (CSharpAttribute attribute in this.Attributes)
            {
                sb.AppendLine(attribute.ToString());
            }

            // Add the first line of the class definition (access modifiers, class name, inheritance)
            IEnumerable<string> parents = this.BaseType == null
                ? this.Interfaces
                : this.BaseType.SingleObjectAsEnumerable().Concat(this.Interfaces);
            sb.Append(AccessModifier.ToCSharpString());
            if (this.IsStatic)
            {
                sb.Append(" static");
            }
            sb.Append($" class {this.Name}");
            if (parents.Any())
            {
                sb.Append(" : ");
                sb.Append(string.Join(", ", parents));
            }
            sb.AppendLine();

            // Start body
            sb.AppendLine("{");

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
                    sb.AppendLine();
                }

                sb.AppendLine(property.ToString().Indent());
            }

            if (this.Properties.Any() && this.Methods.Any())
            {
                sb.AppendLine();
            }

            // Constructors and methods
            isFirst = true;
            foreach (CSharpMethod method in this.Constructors.Concat(this.Methods))
            {
                // Add a new line except for the first property
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.AppendLine();
                }

                sb.AppendLine(method.ToString().Indent());
            }

            // End body
            sb.Append("}");

            // Compile and return result
            string result = sb.ToString();
            return result;
        }
    }
}