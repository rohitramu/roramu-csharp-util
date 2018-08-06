namespace RoRamu.Utils
{
    using System;
    using System.Text;

    public static class ByteUtils
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Converts a string to a byte array using the given encoding.  If no encoding
        /// is provided, <see cref="DefaultEncoding"/> will be used.
        /// </summary>
        /// <param name="str">The string to convert to a byte array</param>
        /// <param name="encoding">The encoding to use when converting - leave this as null if you want to use the default encoding</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] Encode(this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }
            if (encoding == null)
            {
                encoding = ByteUtils.DefaultEncoding;
            }

            return encoding.GetBytes(str);
        }

        public static string DecodeToString(this byte[] data, Encoding encoding = null)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (encoding == null)
            {
                encoding = ByteUtils.DefaultEncoding;
            }

            return encoding.GetString(data);
        }
    }
}
