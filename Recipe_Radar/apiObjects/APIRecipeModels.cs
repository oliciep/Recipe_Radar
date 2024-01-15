﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Radar.apiObjects
{
    public class ExtendedIngredient
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
    }

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
