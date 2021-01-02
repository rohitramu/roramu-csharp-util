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
        /// True if this method should be marked as async, otherwise false.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// The full name of the method's return type.
        /// <para>
        /// If null, the return type will not be shown in the generated output.
        /// </para>
        /// </summary>
        public string ReturnType { get; }

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
        /// <param name="isAsync">True if this method should be marked as async, otherwise false.</param>
        /// <param name="returnType">
        /// The full name of the method's return type.
        /// <para>
        /// If null, the return type will not be shown in the generated output.
        /// </para>
        /// </param>
        /// <param name="parameters">The parameters which need to be provided as inputs to this method.</param>
        /// <param name="body">The method body.</param>
        /// <param name="documentationComment">The method's documentation comment.  The parameters' documentation will automatically be included in this comment.</param>
        public CSharpMethod(
            string name,
            CSharpAccessModifier accessModifier,
            bool isOverride,
            bool isAsync,
            string returnType,
            IEnumerable<CSharpParameter> parameters,
            string body,
            CSharpDocumentationComment documentationComment)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Method name cannot be null or whitespace", nameof(name));
            }
            if (returnType != null && string.IsNullOrWhiteSpace(returnType))
            {
                throw new ArgumentException("Return type cannot be empty or whitespace", nameof(name));
            }

            this.Name = name;
            this.AccessModifier = accessModifier;
            this.IsOverride = isOverride;
            this.IsAsync = isAsync;
            this.ReturnType = returnType;
            this.Parameters = parameters ?? Array.Empty<CSharpParameter>();
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
            this.DocumentationComment = this.Parameters.Any()
                ? new CSharpDocumentationComment(documentationComment.Summary, $"{this.GetParameterDocumentationComment(this.Parameters)}{Environment.NewLine}{documentationComment.RawNotes}")
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
        public virtual string GetMethodSignature()
        {
            StringBuilder sb = new StringBuilder();

            // Things that go before the name of the method
            sb.Append(this.AccessModifier.ToCSharpString());
            if (this.IsOverride)
            {
                sb.Append(" override");
            }
            if (this.IsAsync)
            {
                sb.Append(" async");
            }
            if (this.ReturnType != null)
            {
                sb.Append(' ');
                sb.Append(this.ReturnType);
            }

            // The name of the method
            sb.Append(' ');
            sb.Append(this.Name);

            // The parameters
            sb.Append('(');
            int numParameters = this.Parameters.Count();
            if (numParameters == 1)
            {
                // Check to see if we should put parameters on their own line
                bool isLongList = numParameters > 1;
                bool isFirst = true;
                foreach (CSharpParameter parameter in this.Parameters)
                {
                    if (!isFirst)
                    {
                        // Comma after the previous parameter
                        sb.Append(',');
                    }

                    // Either add newline or space after the previous parameter, depending on whether we want each parameter on their own line
                    if (isLongList)
                    {
                        sb.AppendLine();
                        sb.Append(StringUtils.GetIndentPrefix());
                    }
                    else
                    {
                        sb.Append(' ');
                    }

                    // Add the current parameter
                    sb.Append(parameter.ToString());

                    isFirst = false;
                }
            }
            sb.Append(')');

            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// Converts this abstract representation of a C# method into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# method.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Method signature (access modifiers,
            sb.AppendLine(this.GetMethodSignature());
            sb.AppendLine("{");
            sb.AppendLine(this.Body.Indent());
            sb.Append("}");

            string result = sb.ToString();
            return result;
        }
    }
}
