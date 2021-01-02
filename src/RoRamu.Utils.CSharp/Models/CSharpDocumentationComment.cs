namespace RoRamu.Utils.CSharp
{
    using System.Text;

    /// <summary>
    /// Represents a comment in C#.
    /// </summary>
    public class CSharpDocumentationComment
    {
        private const string LinePrefix = @"/// ";

        /// <summary>
        /// The text in the summary section.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Any text which does not go in the summary section.
        /// </summary>
        public string RawNotes { get; }

        /// <summary>
        /// Creates a new <see cref="CSharpDocumentationComment" /> object.
        /// </summary>
        /// <param name="summary">The text in the summary section.</param>
        /// <param name="rawNotes">Any text which does not go in the summary section.</param>
        public CSharpDocumentationComment(string summary, string rawNotes = null)
        {
            this.Summary = summary;
            this.RawNotes = rawNotes;
        }

        /// <summary>
        /// Converts this abstract representation of a C# documentation comment into a string (i.e. code).
        /// </summary>
        /// <returns>The string representation of this C# documentation comment.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Summary
            if (this.Summary != null)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }
                sb.AppendLine(LinePrefix + @"<summary>");
                sb.AppendLine(CSharpDocumentationComment.MakeCommentString(this.Summary));
                sb.Append(LinePrefix + @"</summary>");
            }

            // Misc
            if (this.RawNotes != null)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }
                sb.Append(CSharpDocumentationComment.MakeCommentString(this.RawNotes));
            }

            return sb.ToString();
        }

        private static string MakeCommentString(string str)
        {
            return str.Indent(indentToken: LinePrefix);
        }
    }
}
