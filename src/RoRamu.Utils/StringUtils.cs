namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Humanizer;
    using Pluralize.NET;

    /// <summary>
    /// Utility methods for working with string objects.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// The pluralizer which can singularize or pluralize words.
        /// </summary>
        private static Pluralizer Pluralizer { get; } = new Pluralizer();

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
                IList<string> lines = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(indentString + line);
                    }
                    else
                    {
                        lines.Add(line);
                    }
                }

                string resultString = string.Join(Environment.NewLine, lines);
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
            if (indentLevel == 0)
            {
                return string.Empty;
            }
            else if (indentLevel > 0)
            {
                StringBuilder sb = new StringBuilder(indentLevel * indentToken.Length);
                for (int i = 0; i < indentLevel; i++)
                {
                    sb.Append(indentToken);
                }

                return sb.ToString();
            }
            else
            {
                throw new ArgumentException("Indent level must be greater than or equal to 0", nameof(indentLevel));
            }
        }

        /// <summary>
        /// Attempts to make the given word plural.
        /// </summary>
        /// <param name="singular">The singular form of the word.</param>
        /// <returns>The plural string if pluralization succeeded, otherwise the original string.</returns>
        public static string Pluralize(this string singular)
        {
            if (singular == null)
            {
                throw new ArgumentNullException(nameof(singular));
            }

            return Pluralizer.Pluralize(singular);
        }

        /// <summary>
        /// Attempts to make the given word singular.
        /// </summary>
        /// <param name="plural">The plural form of the word.</param>
        /// <returns>The singular string if singularization succeeded, otherwise the original string.</returns>
        public static string Singularize(this string plural)
        {
            if (plural == null)
            {
                throw new ArgumentNullException(nameof(plural));
            }

            return Pluralizer.Singularize(plural);
        }

        /// <summary>
        /// Converts the given string into pascal case.
        /// </summary>
        /// <param name="identifier">The string to convert.</param>
        /// <returns>The pascal-case string.</returns>
        public static string Pascalize(this string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return InflectorExtensions.Pascalize(identifier);
        }

        /// <summary>
        /// Converts the given string into camel case.
        /// </summary>
        /// <param name="identifier">The string to convert.</param>
        /// <returns>The camel-case string.</returns>
        public static string Camelize(this string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return InflectorExtensions.Camelize(identifier);
        }

        /// <summary>
        /// Converts the given string so that words are separated by underscores.
        /// </summary>
        /// <param name="identifier">The string to convert.</param>
        /// <returns>The string with words separated by underscores.</returns>
        public static string Underscore(this string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return InflectorExtensions.Underscore(identifier);
        }

        /// <summary>
        /// Converts the given string so that words are separated by hyphens.
        /// </summary>
        /// <param name="identifier">The string to hyphenate.</param>
        /// <returns>The string with words separated by hyphens.</returns>
        public static string Hyphenate(this string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return InflectorExtensions.Hyphenate(identifier);
        }

        /// <summary>
        /// Converts the given string so that words are lowercase and separated by hyphens.
        /// </summary>
        /// <param name="identifier">The string to kebaberize.</param>
        /// <returns>The string with words lowercased and separated by hyphens.</returns>
        public static string Kebaberize(this string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return InflectorExtensions.Kebaberize(identifier);
        }
    }
}
