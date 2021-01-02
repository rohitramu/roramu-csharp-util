namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a C# method.
    /// </summary>
    public class CSharpMethod
    {
        /// <summary>
        /// The name of the method.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The access level of the method.
        /// </summary>
        public CSharpAccessModifier AccessModifier { get; }

        /// <summary>
        /// True if this method is overriding a base type's method, otherwise false.
        /// </summary>
        public bool IsOverride { get; }

        /// <summary>
        /// The method's return type.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// The parameters which need to be provided as inputs to this method.
        /// </summary>
        public IEnumerable<CSharpParameter> Parameters { get; }

        /// <summary>
        /// The method body.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// The documentation comment.
        /// </summary>
        public CSharpDocumentationComment DocumentationComment { get; }

        /// <summary>
        /// Constructs a new <see cref="CSharpMethod" /> object.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="accessModifier">The access level of the method.</param>
        /// <param name="isOverride">True if this method is overriding a base type's method, otherwise false.</param>
        /// <param name="returnType">The method's return type.</param>
        /// <param name="parameters">The parameters which need to be provided as inputs to this method.</param>
        /// <param name="body">The method body.</param>
        /// <param name="documentationComment">The method's documentation comment.  The parameters' documentation will automatically be included in this comment.</param>
        public CSharpMethod(
            string name,
            CSharpAccessModifier accessModifier,
            bool @isOverride,
            Type returnType,
            IEnumerable<CSharpParameter> parameters,
            string body,
            CSharpDocumentationComment documentationComment)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.AccessModifier = accessModifier;
            this.IsOverride = @isOverride;
            this.ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
            this.Parameters = parameters ?? Array.Empty<CSharpParameter>();
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
            this.DocumentationComment = this.Parameters.Any()
                ? new CSharpDocumentationComment(documentationComment.Summary, $"{this.GetParameterDocumentationComment(this.Parameters)}\n{documentationComment.RawNotes}")
                : documentationComment;
        }

        private string GetParameterDocumentationComment(IEnumerable<CSharpParameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            StringBuilder sb = new StringBuilder();
            foreach (CSharpParameter parameter in parameters)
            {
                sb.AppendLine($"<param name=\"{parameter.Name}\">{parameter.Description}</param>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the method signature as a string.
        /// </summary>
        /// <returns>The method signature.</returns>
        public string GetMethodSignature()
        {
            string beforeName = $"{this.AccessModifier.ToCSharpString()} {(this.IsOverride ? "override " : string.Empty)}{this.ReturnType.FullName}";
            string arguments = string.Join(", ", this.Parameters.Select(arg => arg.ToString()));
            if (arguments.Length > 150 && this.Parameters.Count() > 1)
            {
                arguments = "\n" + arguments.Indent();
                arguments = arguments.Replace(",", $",\n{StringUtils.GetIndentPrefix()}");
            }
            string result = $"{beforeName} {this.Name}({arguments})";

            return result;
        }

        /// <summary>
        /// Converts this abstract representation of a C# method into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# method.</returns>
        public override string ToString()
        {
            StringBuilder resultBuilder = new StringBuilder();

            // Method signature (access modifiers,
            resultBuilder.AppendLine(this.GetMethodSignature());
            resultBuilder.AppendLine("{");
            resultBuilder.AppendLine(this.Body.Indent());
            resultBuilder.AppendLine("}");

            string result = resultBuilder.ToString().Trim();

            return result;
        }
    }
}
