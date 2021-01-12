namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a C# constructor method.
    /// </summary>
    public class CSharpClassConstructor : CSharpMethod
    {
        /// <summary>
        /// The parameter values to pass to the base class' constructor, as they will appear in the code.
        /// <para>
        /// If this is null, the base constructor call will not be visible in the generated output.
        /// Do this either when this constructor's class does not inherit from another class, or
        /// when the constructor should call the base class' default constructor automatically.
        /// </para>
        /// </summary>
        public IEnumerable<string> BaseClassConstructorParameterValues { get; }

        /// <summary>
        /// Constructs a new <see cref="CSharpClassConstructor" /> object.
        /// </summary>
        /// <param name="className">The name of the class that contains this constructor.</param>
        /// <param name="accessModifier">The access level of the method.</param>
        /// <param name="parameters">The parameters which need to be provided as inputs to this method.</param>
        /// <param name="baseClassConstructorParameterValues">
        /// The parameter values to pass to the base class' constructor, as they will appear in the code.
        /// <para>
        /// If this is null, the base constructor call will not be visible in the generated output.
        /// Do this either when this constructor's class does not inherit from another class, or
        /// when the constructor should call the base class' default constructor automatically.
        /// </para>
        /// <para>
        /// NOTE: These values will not automatically be sanitized if they are identifiers. For
        /// example, if one of the variables being passed to the base constructor is called "class",
        /// then you should provide the string "@class" instead).
        /// </para>
        /// </param>
        /// <param name="body">The method body.</param>
        /// <param name="documentationComment">
        /// The method's documentation comment. The parameters' documentation will automatically be
        /// included in this comment.
        /// </param>
        public CSharpClassConstructor(
            string className,
            string body,
            CSharpAccessModifier accessModifier = CSharpAccessModifier.Public,
            IEnumerable<CSharpParameter> parameters = null,
            IEnumerable<string> baseClassConstructorParameterValues = null,
            CSharpDocumentationComment documentationComment = null)
            : base(
                name: className,
                returnType: null,
                body: body,
                parameters: parameters,
                accessModifier: accessModifier,
                isStatic: false,
                isOverride: false,
                isAsync: false,
                documentationComment: documentationComment)
        {
            this.BaseClassConstructorParameterValues = baseClassConstructorParameterValues ?? Array.Empty<string>();
        }

        /// <summary>
        /// Gets the method signature as a string.
        /// </summary>
        /// <returns>The method signature.</returns>
        public override string GetMethodSignature()
        {
            // Get the standard method signarure without the base class constructor call
            string result = base.GetMethodSignature();

            // Add the base class constructor call to the signature if required
            if (this.BaseClassConstructorParameterValues != null)
            {
                int numParameters = this.BaseClassConstructorParameterValues.Count();
                if (numParameters == 0)
                {
                    result += " : base()";
                }
                else if (numParameters == 1)
                {
                    result += $" : base({this.BaseClassConstructorParameterValues.Single()})";
                }
                else
                {
                    StringBuilder sb = new StringBuilder(result);
                    sb.AppendLine();
                    sb.Append(StringUtils.GetIndentPrefix());
                    sb.Append(": base(");
                    bool isFirst = true;
                    string indentString = StringUtils.GetIndentPrefix(2);
                    foreach (string parameterValue in this.BaseClassConstructorParameterValues)
                    {
                        if (!isFirst)
                        {
                            sb.Append(',');
                        }

                        sb.AppendLine();
                        sb.Append(indentString);
                        sb.Append(parameterValue);

                        isFirst = false;
                    }
                    sb.Append(')');

                    result += sb.ToString();
                }
            }

            return result;
        }
    }
}
