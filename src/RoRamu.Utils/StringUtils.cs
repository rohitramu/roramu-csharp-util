namespace RoRamu.Utils
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Utility methods for working with string objects.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Indents a string by the desired amount.
        /// </summary>
        /// <param name="stringToIndent">The string to indent.</param>
        /// <param name="indentLevel">The desired indentation level.</param>
        /// <param name="indentToken">The string which represents a single indent.</param>
        /// <returns>The indented string.</returns>
        public static string Indent(this string stringToIndent, int indentLevel = 1, string indentToken = "    ")
        {
            if (stringToIndent == null)
            {
                throw new ArgumentNullException(nameof(stringToIndent));
            }

            // Precalculate the indent string since this won't change
            string indentString = GetIndentPrefix(indentLevel, indentToken);

            // Indent the string using a StringReader and not String.Replace() because
            // we don't know what kind of newline char is used in this string
            using (StringReader reader = new StringReader(stringToIndent))
            {
                StringBuilder sb = new StringBuilder();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.AppendLine(indentString + line);
                }

                string resultString = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                return resultString;
            }
        }

        /// <summary>
        /// Returns a string which can be prefixed to a line of text to produce the desired indentation amount.
        /// </summary>
        /// <param name="indentLevel">The desired level of indentation</param>
        /// <param name="indentToken">The string which represents an indent level of 1</param>
        /// <returns>The string to which a line of text should be appended to produce the desired level of indentation.</returns>
        public static string GetIndentPrefix(int indentLevel = 1, string indentToken = "    ")
        {
            if (indentLevel < 0)
            {
                throw new ArgumentException("Indent level must be greater than or equal to 0", nameof(indentLevel));
            }

            // No indentation
            if (indentLevel == 0)
            {
                return string.Empty;
            }

            // Only 1 level of indentation (optimization to avoid creating an unnecessary StringBuilder)
            if (indentLevel == 1)
            {
                return indentToken;
            }

            // Multiple levels of indentation
            StringBuilder sb = new StringBuilder(indentLevel * indentToken.Length);
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(indentToken);
            }

            return sb.ToString();
        }
    }
}
