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
            this.Body = body ?? string.Empty;
            this.DocumentationComment = documentationComment;

            // Add parameter descriptions into the documentation comment
            string parametersDocumentationComment = this.GetParametersDocumentationComment(this.Parameters);
            if (!string.IsNullOrWhiteSpace(parametersDocumentationComment))
            {
                if (!string.IsNullOrWhiteSpace(this.DocumentationComment?.RawNotes))
                {
                    parametersDocumentationComment += $"{Environment.NewLine}{this.DocumentationComment.RawNotes}";
                }

                this.DocumentationComment = new CSharpDocumentationComment(this.DocumentationComment?.Summary, parametersDocumentationComment);
            }
        }

        private string GetParametersDocumentationComment(IEnumerable<CSharpParameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (!parameters.Any())
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (CSharpParameter parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Description))
                {
                    if (!first)
                    {
                        sb.AppendLine();
                    }

                    sb.Append($"<param name=\"{parameter.Name}\">{parameter.Description}</param>");
                }

                first = false;
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
            if (numParameters == 1) // If we only have 1 parameter, don't put it on its own line
            {
                sb.Append(this.Parameters.Single().ToString());
            }
            else if (numParameters > 1) // If we have more than 1 parameter, put each parameter on its own line
            {
                bool isFirst = true;
                foreach (CSharpParameter parameter in this.Parameters)
                {
                    if (!isFirst)
                    {
                        // Comma after the previous parameter
                        sb.Append(',');
                    }

                    // Put each parameter on its own line
                    sb.AppendLine();

                    // Add the current parameter
                    sb.Append(CSharpNamingUtils.SanitizeIdentifier(parameter.ToString()).Indent());

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

            sb.AppendLine(this.DocumentationComment.ToString());
            sb.AppendLine(this.GetMethodSignature());
            sb.AppendLine("{");
            sb.AppendLine(this.Body.Indent());
            sb.Append("}");

            string result = sb.ToString();
            return result;
        }
    }
}
