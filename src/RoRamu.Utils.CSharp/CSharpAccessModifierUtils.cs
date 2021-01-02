namespace RoRamu.Utils.CSharp
{
    using System;

    /// <summary>
    /// Utility methods for interacting with C# access modifiers.
    /// </summary>
    public static class CSharpAccessModifierUtils
    {
        /// <summary>
        /// Converts an access modifier into the equivalent C# string (i.e. code).
        /// </summary>
        /// <param name="accessModifier">The access modifier to convert.</param>
        /// <returns>The string representation of this access modifier.</returns>
        public static string ToCSharpString(this CSharpAccessModifier accessModifier)
        {
            switch (accessModifier)
            {
                case CSharpAccessModifier.Public: return "public";
                case CSharpAccessModifier.Protected: return "protected";
                case CSharpAccessModifier.Internal: return "internal";
                case CSharpAccessModifier.Private: return "private";
                default: throw new ArgumentException("Unknown access modifier", nameof(accessModifier));
            }
        }
    }
}