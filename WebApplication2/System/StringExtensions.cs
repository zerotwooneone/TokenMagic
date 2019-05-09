using System;
using System.Collections.Generic;

namespace WebApplication2.System
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
        }

        public static IEnumerable<string> Chunk(this string str, int maxChunkSize) {
            for (int i = 0; i < str.Length; i += maxChunkSize) 
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length-i));
        }
    }
}