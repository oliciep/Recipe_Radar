using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRadar
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageType { get; set; }
    }

    public class RootObject
    {
        public Recipe[]? Results { get; set; }
        // Add other properties as needed for your API response
    }
}
