namespace RoRamu.Utils.CSharp
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// Utility methods for C# documentation.
    /// </summary>
    public static class CSharpDocumentationUtils
    {
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
                XmlDocument doc = new XmlDocument();
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

            string path = "T:" + type.GetCSharpName();
            string documentation = GetDocumentationComment(xmlDocumentationFile, path);

            return documentation;
        }

        /// <summary>
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
            string path = $"{prefix}:" + member.DeclaringType.GetCSharpName() + "." + member.Name;

            // Get the documentation comment
            string documentation = GetDocumentationComment(xmlDocumentationFile, path);

            return documentation;
        }

        private static string GetDocumentationComment(XmlDocument xmlDocumentationFile, string path)
        {
            string documentation = xmlDocumentationFile
                .SelectSingleNode("//member[starts-with(@name, '" + path + "')]")?
                .InnerXml;

            return documentation;
        }
    }
}