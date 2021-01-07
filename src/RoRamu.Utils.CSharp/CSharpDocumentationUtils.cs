namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// Utility methods for C# documentation.
    /// </summary>
    public static class CSharpDocumentationUtils
    {
        private static readonly Regex LeadingWhitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);

        /// <summary>
        /// Tries to get the documentation file for a given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the documentation file for.</param>
        /// <param name="xmlDocumentationFile">The XML file which is likely to be the documentation file for the given assembly.</param>
        /// <returns>True if the documentation file was found, otherwise false.</returns>
        public static bool TryGetXmlDocumentationFile(this Assembly assembly, out XmlDocument xmlDocumentationFile)
        {
            try
            {
                // Get the path to the DLL which contains the interface type
                string dllPath = assembly.Location;
                if (string.IsNullOrWhiteSpace(dllPath) || !File.Exists(dllPath))
                {
                    xmlDocumentationFile = null;
                    return false;
                }
                dllPath = Path.GetFullPath(dllPath);

                // Find the XML documentation file
                string xmlPath = Path.ChangeExtension(dllPath, ".xml");
                if (!File.Exists(xmlPath))
                {
                    xmlDocumentationFile = null;
                    return false;
                }

                // Parse the file as an XML document
                XmlDocument doc = new XmlDocument
                {
                    PreserveWhitespace = true
                };
                doc.Load(xmlPath);

                xmlDocumentationFile = doc;
                return true;
            }
            catch (Exception e)
            {
                // TODO: log
                Console.Error.WriteLine(e);

                xmlDocumentationFile = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the documentation comment for the given type in the given XML documentation file.
        /// </summary>
        /// <param name="type">The type to get the documentation comment for.</param>
        /// <param name="xmlDocumentationFile">The XML documentation file.</param>
        /// <returns>The XML documentation comment if it was found, otherwise null.</returns>
        public static string GetDocumentationComment(this Type type, XmlDocument xmlDocumentationFile)
        {
            if (xmlDocumentationFile == null)
            {
                throw new ArgumentNullException(nameof(xmlDocumentationFile));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            string path = "T:" + type.GetCSharpName(identifierOnly: true);
            string documentation = GetDocumentationComment(xmlDocumentationFile, path);

            return documentation;
        }

        ///  <summary>
        /// Tries to get the documentation comment for the given method in the given XML documentation file.
        /// </summary>
        /// <param name="member">The method to get the documentation comment for.</param>
        /// <param name="xmlDocumentationFile">The XML documentation file.</param>
        /// <returns>The XML documentation comment if it was found, otherwise null.</returns>
        public static string GetDocumentationComment(this MemberInfo member, XmlDocument xmlDocumentationFile)
        {
            if (xmlDocumentationFile == null)
            {
                throw new ArgumentNullException(nameof(xmlDocumentationFile));
            }
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            // Get the correct prefix for the member type
            string prefix;
            switch (member.MemberType)
            {
                case MemberTypes.Method:
                case MemberTypes.Constructor:
                    prefix = "M";
                    break;
                case MemberTypes.Property:
                    prefix = "P";
                    break;
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    prefix = "T";
                    break;
                default:
                    prefix = null;
                    break;
            }

            // If we don't know the prefix for the member type, then give up
            if (prefix == null)
            {
                return null;
            }

            // Create the XML path
            string path = $"{prefix}:" + member.DeclaringType.GetCSharpName(identifierOnly: true) + "." + member.Name;

            // Get the documentation comment
            string documentation = GetDocumentationComment(xmlDocumentationFile, path);

            return documentation;
        }

        private static string GetDocumentationComment(XmlDocument xmlDocumentationFile, string path)
        {
            // Get the raw text
            string documentation = xmlDocumentationFile
                .SelectSingleNode("//member[starts-with(@name, '" + path + "')]")?
                .InnerXml;

            // Return early if the documentation couldn't be found
            if (string.IsNullOrWhiteSpace(documentation))
            {
                return null;
            }

            // Remove any trailing whitespace
            documentation = documentation.TrimEnd();

            // Remove indentation of the text block
            string[] lineArray = documentation
                .Split('\n')
                .SkipWhile(s => string.IsNullOrWhiteSpace(s))
                .ToArray();
            if (lineArray.Length > 0)
            {
                // Iterate over the characters in each line until we find one that doesn't match the other lines or isn't a whitespace
                int charIndex = 0;
                bool reachedEnd = false;
                bool foundWhitespacePrefix = false;
                while (!reachedEnd && !foundWhitespacePrefix)
                {
                    reachedEnd = true;
                    char currentChar = '\0';
                    for (int i = 0; i < lineArray.Length && !foundWhitespacePrefix; i++)
                    {
                        string line = lineArray[i];
                        if (line.Length <= charIndex)
                        {
                            continue;
                        }

                        if (currentChar == '\0')
                        {
                            currentChar = line[charIndex];
                        }

                        reachedEnd = false;

                        char current = line[charIndex];
                        if (current != currentChar || !char.IsWhiteSpace(current))
                        {
                            foundWhitespacePrefix = true;
                        }
                    }

                    charIndex++;
                }

                if (foundWhitespacePrefix)
                {
                    // Replace each line with a version that doesn't have the prefixed whitespaces
                    for (int i = 0; i < lineArray.Length; i++)
                    {
                        if (lineArray[i].Length <= i)
                        {
                            lineArray[i] = string.Empty;
                        }
                        else
                        {
                            string newLine = lineArray[i].Substring(charIndex - 1);
                            lineArray[i] = newLine;
                        }
                    }

                    documentation = string.Join("\n", lineArray);
                }
            }

            return documentation;
        }
    }
}