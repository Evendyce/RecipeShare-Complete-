using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeShare.Models.Shared
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description or tooltip to explain the tag's intent or meaning.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Optional HEX or named color for display purposes. (e.g., "#FF5733" or "red")
        /// </summary>
        public string? Color { get; set; }
    }
}
