using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// File used for storing object definitions from complexSearch API Request
/// </summary>
namespace Recipe_Radar.apiObjects
{
    /// <summary>
    /// Class definition for storing Recipe information
    /// </summary>
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageType { get; set; }
    }

    /// <summary>
    // Class definition for storing JSON information retrieved from API request
    /// </summary>
    public class RootObject
    {
        public Recipe[]? Results { get; set; }
    }
}
