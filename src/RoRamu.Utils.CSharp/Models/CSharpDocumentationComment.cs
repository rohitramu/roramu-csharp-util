namespace RoRamu.Utils.CSharp
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a comment in C#.
    /// </summary>
    public class CSharpDocumentationComment : CSharpComment
    {
        /// <inheritdoc />
        protected override string LinePrefix { get; } = @"/// ";

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
        public CSharpDocumentationComment(string summary, string rawNotes = null) : base(CombineDocumentationParts(summary, rawNotes))
        {
            this.Summary = summary;
            this.RawNotes = rawNotes;
        }

        private static string CombineDocumentationParts(string summary, string rawNotes)
        {
            StringBuilder sb = new StringBuilder();
            if (summary != null)
            {
                sb.AppendLine("<summary>");
                sb.AppendLine(summary);
                sb.Append("</summary>");
            }

            if (rawNotes != null)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                sb.Append(rawNotes);
            }

            string result = sb.ToString();
            return result;
        }
    }
}
