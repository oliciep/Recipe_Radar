using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// File used for storing object definitions from getRecipeInformation API Request
/// </summary>
namespace Recipe_Radar.apiObjects
{
    /// <summary>
    /// Class definition for individual ingredient information, stored as list in RecipeInformation
    /// </summary>
    public class ExtendedIngredient
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
    }

    /// <summary>
    /// Class definition for storing JSON information retrieved from API request, stores recipe information as well as extended ingredient list
    /// </summary>
    public class RecipeInformation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReadyInMinutes { get; set; }
        public int Servings { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public List<ExtendedIngredient> ExtendedIngredients { get; set; }
    }
}
