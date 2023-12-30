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
using RecipeRadar;
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

            FindButton.Click += FindButton_Click;
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
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
    }
}