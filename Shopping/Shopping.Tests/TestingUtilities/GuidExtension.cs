namespace Shopping.Tests.TestingUtilities
{
    public static class GuidExtension
    {
        /// <summary>
        /// Converts an integer to a GUID by placing its decimal representation in the last 12 hex digits.
        /// </summary>
        /// <param name="value">The integer value (0 to 999,999,999,999)</param>
        /// <returns>A GUID in the format: 00000000-0000-0000-0000-{value:000000000000}</returns>
        /// <example>123.ToGuid() returns 00000000-0000-0000-0000-000000000123</example>
        public static Guid ToGuid(this int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative");
            
            return new Guid($"00000000-0000-0000-0000-{value:000000000000}");
        }
    }
}