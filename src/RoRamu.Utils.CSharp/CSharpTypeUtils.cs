namespace RoRamu.Utils.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Utility extension methods for <see cref="Type" /> objects.
    /// </summary>
    public static class CSharpTypeUtils
    {
        /// <summary>
        /// Gets the underlying type for an array type, and also returns the number of dimensions.
        /// </summary>
        /// <param name="arrayType">The type that represents an array</param>
        /// <param name="dimensions">The number of dimensions - if the provided type is not an array type, this will be zero</param>
        /// <returns>
        /// The underlying type (i.e. the type when all dimensions are removed and it is no longer an array).
        /// If the provided type is not an array type, this method will return the provided type.
        /// </returns>
        public static Type GetArrayElementType(Type arrayType, out int dimensions)
        {
            if (arrayType == null)
            {
                throw new ArgumentNullException(nameof(arrayType));
            }

            // Create a temporary type so we don't lose the reference to the passed-in type
            Type tempType = arrayType;

            // Unwrap the array
            int arrayDimensions = 0;
            while (tempType.IsArray)
            {
                tempType = tempType.GetElementType();
                arrayDimensions++;
            }

            dimensions = arrayDimensions;
            return tempType;
        }

        /// <summary>
        /// Converts a type to a C# string that can be compiled in code.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="identifierOnly">Omits any type parameters, array indicators or enclosing type.</param>
        /// <param name="includeNamespace">Includes the namespace in the name.</param>
        /// <returns>The C# string</returns>
        public static string GetCSharpName(this Type type, bool identifierOnly = false, bool includeNamespace = true)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // Unwrap the array type if required
            Type tempType = CSharpTypeUtils.GetArrayElementType(type, out int arrayDimensions);

            // Get the C# type name
            string typeName;
            if (tempType == typeof(void))
            {
                return "void";
            }
            else if (!tempType.IsGenericType)
            {
                typeName = tempType.Name;
            }
            else
            {
                // Get the full name of the type without it's generic arguments (i.e. everything before the backtick)
                string typeNameWithoutArguments = tempType.Name.Substring(0, tempType.Name.IndexOf('`'));

                // If this is a definition and not the actual type, just return the name without the type parameters
                if (identifierOnly)
                {
                    typeName = typeNameWithoutArguments;
                }
                else
                {
                    // Get the type arguments
                    IEnumerable<string> typeArguments = tempType.GenericTypeArguments.Select(typeArgument => typeArgument.GetCSharpName());
                    string typeArgumentsString = string.Join(", ", typeArguments);

                    // Create the full type name with arguments
                    typeName = $"{typeNameWithoutArguments}<{typeArgumentsString}>";
                }
            }

            // Check if this type is nested inside another type
            if (!identifierOnly && tempType.DeclaringType != null)
            {
                // Prepend the declaring type
                typeName = $"{tempType.DeclaringType.GetCSharpName()}.{typeName}";
            }
            else if (includeNamespace && !string.IsNullOrEmpty(tempType.Namespace))
            {
                // Add the namespace
                typeName = $"{tempType.Namespace}.{typeName}";
            }

            // Add the array brackets back into the type name
            while (!identifierOnly && arrayDimensions > 0)
            {
                typeName += "[]";
                arrayDimensions--;
            }

            return typeName;
        }
    }
}