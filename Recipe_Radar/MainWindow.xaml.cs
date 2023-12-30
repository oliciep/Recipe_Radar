using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using static RecipeRadar.MainWindow;

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            // Subscribe to button click events
            ApplyButton.Click += ApplyButton_Click;
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            string? apiKey = "288d4b3d9f8d44d39f041ea5260c4301";
            string ingredients = "beef";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                HttpResponseMessage response = await client.GetAsync($"recipes/search?query={ingredients}&number=1&apiKey={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    RootObject? rootObject = JsonConvert.DeserializeObject<RootObject>(responseData);
                    String outputRecipes = new("");

                    foreach (var recipe in rootObject.Results)
                    {
                        outputRecipes = recipe.Title;
                    }
                    MessageBox.Show(outputRecipes);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve data");
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Your logic for the Reset button click event
            // For example:
            MessageBox.Show("Reset button clicked!");
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Your logic for the Refresh button click event
            // For example:
            MessageBox.Show("Refresh button clicked!");
        }

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
}