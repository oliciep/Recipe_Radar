using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRadar
{
    public class Recipe
    {
        public string? Title { get; set; }
        public int ReadyInMinutes { get; set; }
        public int Servings { get; set; }
        // Add other properties as needed for your API response
    }

    public class RootObject
    {
        public Recipe[]? Results { get; set; }
        // Add other properties as needed for your API response
    }
}
