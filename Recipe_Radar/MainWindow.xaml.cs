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
using Recipe_Radar.testData;
using static RecipeRadar.MainWindow;
using static Recipe_Radar.testData.RecipeTests;
using Recipe_Radar.apiObjects;
using System.Xml;
using System.Text.RegularExpressions;

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecipes = 1;
        private int maxTimeAllowed = 60;
        private bool isDialogShown = false;
        private RootObject? fetchedRecipes;

        public MainWindow()
        {

            InitializeComponent();

            FindButton.Click += FindButton_Click;
            RecipesComboBox.SelectedIndex = 0;
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            // Real Data (WITH API)
            StringBuilder ingredients = new("");
            foreach (var item in ingredientListBox.Items)
            {
                ingredients.Append(item + ", ");
            }
            string? apiKey = APIKeys.SpoonacularKey;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                HttpResponseMessage response = await client.GetAsync($"recipes/complexSearch?query={ingredients}&maxReadyTime={maxTimeAllowed}&number={numberOfRecipes}&apiKey={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    fetchedRecipes = JsonConvert.DeserializeObject<RootObject>(responseData);
                    createWindow(fetchedRecipes);
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

        private void createWindow(RootObject rootObject)
        {
            var imageWindow = new Window();

            imageWindow.Title = "Your Recipes";
            imageWindow.Width = 800;
            imageWindow.Height = 600;
            imageWindow.Background = Brushes.LightGreen;
            imageWindow.Closed += ImageWindow_Closed;
            fetchResults(fetchedRecipes, imageWindow);
        }

        private void fetchResults(RootObject rootObject, Window imageWindow)
        {
            List<string> recipeImages = new List<string>();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = null;
            var stackPanel = new StackPanel();

            foreach (var recipe in rootObject.Results)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(recipe.Image));
                TextBlock textBlock = new TextBlock();
                Button chooseButton = new Button();

                textBlock.Inlines.Add(new Run("Recipe: ") { Foreground = Brushes.DarkGreen });
                textBlock.Inlines.Add(new Run($"{recipe.Title}") { Foreground = Brushes.Olive });
                textBlock.FontSize = 18;
                textBlock.Foreground = Brushes.DarkOliveGreen;
                textBlock.Margin = new Thickness(10);
                textBlock.TextAlignment = TextAlignment.Center;

                Image img = new Image();
                img.Source = bitmap;
                img.Width = 300;
                img.Height = 200;
                img.Margin = new Thickness(10);

                chooseButton.Content = $"Choose Recipe";
                chooseButton.Style = (Style)Resources["ButtonStyle"];
                chooseButton.Tag = recipe.Id;
                chooseButton.Click += chooseButton_Click;

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(img);
                stackPanel.Children.Add(chooseButton);
            }

            scrollViewer.Content = stackPanel;

            imageWindow.Title = "Your Recipes";
            imageWindow.Width = 800;
            imageWindow.Height = 600;
            imageWindow.Background = Brushes.LightGreen;
            imageWindow.Content = scrollViewer;
            if (!isDialogShown)
            {
                isDialogShown = true;
                imageWindow.ShowDialog();
            }
        }

        private async void chooseRecipe(Window window, int uniqueID)
        {
            string? apiKey = APIKeys.SpoonacularKey;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                HttpResponseMessage response = await client.GetAsync($"recipes/{uniqueID}/information?apiKey={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    RecipeInformation? recipeInfo = JsonConvert.DeserializeObject<RecipeInformation>(responseData);
                    fetchInfo(window,recipeInfo);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve data");
                }
            }
        }

        private void fetchInfo(Window window, RecipeInformation recipeInformation)
        {
            window.Title = $"Recipe: {recipeInformation.Title}";

            StringBuilder ingredientsList = new("Ingredients:\n");
            foreach (var ingredient in recipeInformation.ExtendedIngredients)
            {
                ingredientsList.Append("•" + ingredient.Name + "\n");
            }

            StringBuilder instructionsList = new("Instructions:\n");
            string filterInstructions = Regex.Replace(recipeInformation.Instructions.ToString(), @"<ol>|</ol>", "", RegexOptions.IgnoreCase);
            filterInstructions = Regex.Replace(filterInstructions, @"<li>|</li>", "", RegexOptions.IgnoreCase);
            string[] instructions = filterInstructions.Split('.');
            foreach (var instruction in instructions)
            {
                instructionsList.Append("•" + instruction + "\n");
            }

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            var stackPanel = new StackPanel();

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Recipe: ") { Foreground = Brushes.DarkGreen });
            titleBlock.Inlines.Add(new Run($"{recipeInformation.Title}") { Foreground = Brushes.Olive });
            titleBlock.FontSize = 24;
            titleBlock.Margin = new Thickness(10);
            titleBlock.TextAlignment = TextAlignment.Center;

            BitmapImage bitmap = new BitmapImage(new Uri(recipeInformation.Image));
            Image recipeImage = new Image();
            recipeImage.Source = bitmap;
            recipeImage.Width = 400;
            recipeImage.Height = 300;
            recipeImage.Margin = new Thickness(10);
            recipeImage.VerticalAlignment = VerticalAlignment.Top;

            TextBlock infoBlock = new TextBlock();
            infoBlock.Text = $"Ready in: {recipeInformation.ReadyInMinutes} minutes. \n Serves: {recipeInformation.Servings} people.";
            infoBlock.FontSize = 18;
            infoBlock.Foreground = Brushes.DarkOliveGreen;
            infoBlock.Margin = new Thickness(10);
            infoBlock.TextAlignment = TextAlignment.Center;

            TextBlock ingredientsBlock = new TextBlock();
            ingredientsBlock.Text = ingredientsList.ToString();
            ingredientsBlock.FontSize = 12;
            ingredientsBlock.Foreground = Brushes.DarkOliveGreen;
            ingredientsBlock.Margin = new Thickness(10);
            ingredientsBlock.TextAlignment = TextAlignment.Center;
            ingredientsBlock.VerticalAlignment = VerticalAlignment.Top;

            Button returnButton = new Button();
            returnButton.Content = "Return to Recipe Select";
            returnButton.Style = (Style)Resources["ButtonStyle"];
            returnButton.Tag = window;
            returnButton.Click += returnButton_Click;

            var ingredientsPanel = new StackPanel();
            ingredientsPanel.Orientation = Orientation.Horizontal;
            ingredientsPanel.Children.Add(recipeImage);
            ingredientsPanel.Children.Add(ingredientsBlock);

            TextBlock instructionsBlock = new TextBlock();
            instructionsBlock.Text = instructionsList.ToString();
            instructionsBlock.FontSize = 12;
            instructionsBlock.Foreground = Brushes.DarkOliveGreen;
            instructionsBlock.Margin = new Thickness(10);
            instructionsBlock.TextAlignment = TextAlignment.Center;

            stackPanel.Children.Add(titleBlock);
            stackPanel.Children.Add(ingredientsPanel);
            stackPanel.Children.Add(infoBlock);
            stackPanel.Children.Add(instructionsBlock);
            stackPanel.Children.Add(returnButton);

            scrollViewer.Content = stackPanel;
            window.Content = scrollViewer;


        }

            private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string newIngredient = ingredientsTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newIngredient))
            {
                ingredientListBox.Items.Add(newIngredient);
                ingredientsTextBox.Text = string.Empty;
            }
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (ingredientListBox.SelectedItem != null)
            {
                ingredientListBox.Items.Remove(ingredientListBox.SelectedItem);
            }
        }

        private void RecipesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private void chooseButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            int uniqueID = Convert.ToInt32(button.Tag);
            if (button != null)
            {
                Window window = Window.GetWindow(button);

                if (window != null)
                {
                    if (window.Content is ScrollViewer scrollViewer)
                    {
                        scrollViewer.Content = null;

                    }
                    chooseRecipe(window, uniqueID);
                }
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                Window window = Window.GetWindow(button);

                if (window != null)
                {
                    fetchResults(fetchedRecipes, window);
                }
            }
        }

        private void ImageWindow_Closed(object sender, EventArgs e)
        {
            isDialogShown = false;
        }

    }
}