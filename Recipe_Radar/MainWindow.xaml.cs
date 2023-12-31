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
using Recipe_Radar.config;
using RecipeRadar;
using Recipe_Radar.testData;
using static RecipeRadar.MainWindow;
using static Recipe_Radar.testData.RecipeTests;

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecipes = 1;
        private int maxTimeAllowed = 60;
        private String ingredients = "";

        public MainWindow()
        {

            InitializeComponent();

            FindButton.Click += FindButton_Click;
            RecipesComboBox.SelectedIndex = 0;
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            // Real Data (WITH API)
            string? apiKey = APIKeys.SpoonacularKey;
            using (HttpClient client = new HttpClient())

            {
                RecipeTests recipeTests = new RecipeTests();
                RecipeTests.TestRootObject testData = recipeTests.TestRecipeData();
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                HttpResponseMessage response = await client.GetAsync($"recipes/complexSearch?query={ingredients}&maxReadyTime={maxTimeAllowed}&number={numberOfRecipes}&apiKey={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    RootObject? rootObject = JsonConvert.DeserializeObject<RootObject>(responseData);
                    StringBuilder outputRecipes = new("");
                    fetchResults(rootObject);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve data");
                }
            }

            // Test Data (NO API)
            /*
            RecipeTests recipeTests = new RecipeTests();
            RecipeTests.TestRootObject testData = recipeTests.TestRecipeData();
            StringBuilder outputRecipes = new("");

            foreach (var recipe in testData.Results)
            {
                outputRecipes.Append("Title: " +recipe.Title + "\n\n");
            }
            fetchResults(testData);
            */
        }

        private void fetchResults(RootObject rootObject)
        {
            List<string> recipeImages = new List<string>();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            var imageWindow = new Window();
            var stackPanel = new StackPanel();

            foreach (var recipe in rootObject.Results)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(recipe.Image));
                TextBlock textBlock = new TextBlock();

                textBlock.Text = $"Title: {recipe.Title}";
                textBlock.FontSize = 18;
                textBlock.Foreground = Brushes.DarkOliveGreen;
                textBlock.Margin = new Thickness(10);
                textBlock.TextAlignment = TextAlignment.Center;

                Image img = new Image();
                img.Source = bitmap;
                img.Width = 300;
                img.Height = 200;
                img.Margin = new Thickness(10);

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(img);
            }

            scrollViewer.Content = stackPanel;

            imageWindow.Title = "Your Recipes";
            imageWindow.Width = 800;
            imageWindow.Height = 600;
            imageWindow.Background = Brushes.LightGreen;
            imageWindow.Content = scrollViewer;
            imageWindow.ShowDialog();
        }

    private void ingredientsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ingredients = ingredientsTextBox.Text.Trim();
            if (string.IsNullOrEmpty(ingredients))
            {
                MessageBox.Show("Please enter ingredients.");
            }
        }

        private void RecipesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Retrieve the selected value from ComboBox and update numberOfRecipes
            if (RecipesComboBox.SelectedItem != null)
            {
                int.TryParse(((ComboBoxItem)RecipesComboBox.SelectedItem).Content.ToString(), out numberOfRecipes);
            }
        }

        private void timeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String maxTime = timeTextBox.Text.Trim();
            if (int.TryParse(timeTextBox.Text.Trim(), out int result))
            {
                maxTimeAllowed = result;
            }
            if (string.IsNullOrEmpty(maxTime))
            {
                MessageBox.Show("Please enter maximum ready time.");
            }
        }
    }
}