using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Radar.testData
{
    public class RecipeTests
    {
        public class Result
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
            public string ImageType { get; set; }
        }

        public class TestRootObject
        {
            public int Offset { get; set; }
            public int Number { get; set; }
            public List<Result> Results { get; set; }
            public int TotalResults { get; set; }
        }

        // Test Data
        public TestRootObject TestRecipeData()
        {
            var testRoot = new TestRootObject
            {
                Offset = 0,
                Number = 2,
                Results = new List<Result>
                {
                    new Result
                    {
                        Id = 716429,
                        Title = "Pasta with Garlic, Scallions, Cauliflower & Breadcrumbs",
                        Image = "https://spoonacular.com/recipeImages/716429-312x231.jpg",
                        ImageType = "jpg"
                    },
                    new Result
                    {
                        Id = 715538,
                        Title = "What to make for dinner tonight?? Bruschetta Style Pork & Pasta",
                        Image = "https://spoonacular.com/recipeImages/715538-312x231.jpg",
                        ImageType = "jpg"
                    }
                },
                TotalResults = 86
            };
            return testRoot;
        }
    }
}
