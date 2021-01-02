namespace RoRamu.Utils.CSharp
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Utility methods for C# names (e.g. identifier names).
    /// </summary>
    public static class CSharpNamingUtils
    {
        /// <summary>
        /// Checks whether the provided string is a reserved C# keyword.
        /// </summary>
        /// <param name="identifier">The identifier to check</param>
        /// <returns>True if the string is a reserved keyword, otherwise false.</returns>
        public static bool IsValidIdentifier(string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return SyntaxFacts.IsValidIdentifier(identifier);
        }

        /// <summary>
        /// Sanitizes a C# string if required, otherwise returns the original string.
        /// </summary>
        /// <param name="identifier">The identifier to sanitize</param>
        /// <returns>The sanitized C# keyword which is safe to use as an identifier.</returns>
        public static string SanitizeIdentifier(string identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (CSharpNamingUtils.IsValidIdentifier(identifier))
            {
                return identifier;
            }
            else
            {
                // Add an "@" to escape keywords
                string result = $"@{identifier}";

                // Make sure that it is now valid - if it isn't, it was never a C# keyword to begin with.
                // It was just an invalid identifier, probably with special characters or numbers.
                if (!CSharpNamingUtils.IsValidIdentifier(result))
                {
                    throw new ArgumentException($"Invalid characters found in identifier '{identifier}'", nameof(identifier));
                }

                return result;
            }
        }
    }
}