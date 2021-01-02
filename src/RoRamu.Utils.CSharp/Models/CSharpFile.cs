namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a C# code file.
    /// </summary>
    public class CSharpFile
    {
        /// <summary>
        /// The namespace that everything in this file will be under.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The usings in this file.
        /// </summary>
        public IEnumerable<string> Usings { get; }

        /// <summary>
        /// The classes included in this file.
        /// </summary>
        public IEnumerable<CSharpClass> Classes { get; }

        /// <summary>
        /// The header at the top of this file.
        /// </summary>
        public CSharpDocumentationComment FileHeader { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpFile" /> object.
        /// </summary>
        /// <param name="namespace">The namespace that everything in this file will be under.</param>
        /// <param name="usings">The usings in this file.</param>
        /// <param name="classes">The classes included in this file.</param>
        /// <param name="fileHeader">The header at the top of this file.</param>
        public CSharpFile(string @namespace, IEnumerable<string> usings, IEnumerable<CSharpClass> classes, CSharpDocumentationComment fileHeader)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentException("Namespace name cannot be null or whitespace", nameof(@namespace));
            }

            this.Namespace = @namespace;
            this.Usings = usings == null
                ? Array.Empty<string>().AsEnumerable()
                : new HashSet<string>(usings);
            this.Classes = classes ?? Array.Empty<CSharpClass>();
            this.FileHeader = fileHeader;
        }

        /// <summary>
        /// Converts this abstract representation of a C# file into a string representation (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# file.</returns>
        public override string ToString()
        {
            StringBuilder resultBuilder = new StringBuilder();

            // File header
            if (this.FileHeader != null)
            {
                resultBuilder.AppendLine(this.FileHeader.ToString());
                resultBuilder.AppendLine();
            }

            // Namespace
            resultBuilder.AppendLine($"namespace {this.Namespace}");

            // Start body
            resultBuilder.AppendLine("{");

            // Usings
            foreach (string @using in this.Usings)
            {
                resultBuilder.AppendLine($"using {@using};".Indent());
            }

            if (this.Usings.Any() && this.Classes.Any())
            {
                resultBuilder.AppendLine();
            }

            // Classes
            bool isFirst = true;
            foreach (CSharpClass @class in this.Classes)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    resultBuilder.AppendLine();
                }

                resultBuilder.AppendLine(@class.ToString().Indent());
            }

            // End body
            resultBuilder.AppendLine("}");

            // Compile and return result
            string result = resultBuilder.ToString().Trim();
            return result;
        }
    }
}
