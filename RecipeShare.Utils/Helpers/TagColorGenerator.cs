using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecipeShare.Utils.Helpers
{
    public static class TagColorGenerator
    {
        /// <summary>
        /// Generates a HEX color code based on a tag name, with fallback random if empty.
        /// </summary>
        public static string GenerateColor(string? tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return GetRandomColor();

            // Use SHA256 hash of the tag name to produce consistent pseudo-random colors
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(tagName));

            // Use first 3 bytes as RGB components
            var r = bytes[0];
            var g = bytes[1];
            var b = bytes[2];

            // Slight pastel adjustment for readability (avoid super-dark or too-bright)
            r = (byte)((r + 128) / 2);
            g = (byte)((g + 128) / 2);
            b = (byte)((b + 128) / 2);

            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private static string GetRandomColor()
        {
            var rng = RandomNumberGenerator.GetBytes(3);
            return $"#{rng[0]:X2}{rng[1]:X2}{rng[2]:X2}";
        }
    }
}
