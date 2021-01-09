namespace RoRamu.Utils.CSharp
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a comment in C#.
    /// </summary>
    public class CSharpComment
    {
        /// <summary>
        /// The prefix to use for each line of the comment.
        /// </summary>
        protected virtual string LinePrefix { get; } = @"// ";

        /// <summary>
        /// Any text which does not go in the summary section.
        /// </summary>
        public virtual string Text { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpDocumentationComment" /> object.
        /// </summary>
        /// <param name="text">The comment text.</param>
        public CSharpComment(string text = null)
        {
            this.Text = text ?? string.Empty;
        }

        /// <summary>
        /// Converts this abstract representation of a C# documentation comment into a string (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# documentation comment.</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                return this.Text.Indent(indentToken: this.LinePrefix);
            }

            // Trim the whitespace from the end of each line, and also add the comment prefix
            StringBuilder sb = new StringBuilder();
            using (StringReader reader = new StringReader(this.Text))
            {
                string line = reader.ReadLine();

                // Skip leading blank lines
                while (line != null && string.IsNullOrWhiteSpace(line))
                {
                    // Do nothing
                    line = reader.ReadLine();
                }

                // Indent the rest of the lines
                bool first = true;
                while (line != null)
                {
                    if (!first)
                    {
                        sb.AppendLine();
                    }
                    first = false;

                    string outputLine = line.Indent(indentToken: this.LinePrefix);
                    sb.Append(outputLine);

                    line = reader.ReadLine();
                }
            }

            string result = sb.ToString();
            return result;
        }
    }
}
