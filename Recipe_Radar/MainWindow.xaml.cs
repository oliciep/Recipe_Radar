﻿using System.Net.Http;
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

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecipes = 1;
        private int maxTimeAllowed = 60;

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
                    RootObject? rootObject = JsonConvert.DeserializeObject<RootObject>(responseData);
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
                Button chooseButton = new Button();

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

                chooseButton.Content = $"Choose {recipe.Id}";
                chooseButton.Width = 100;
                chooseButton.Margin = new Thickness(10);
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
            imageWindow.ShowDialog();
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
            var stackPanel = new StackPanel();
            TextBlock textBlock = new TextBlock();
            BitmapImage bitmap = new BitmapImage(new Uri(recipeInformation.Image));

            textBlock.Text = $"Title: {recipeInformation.Title}";
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

            window.Content = stackPanel;
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
                    window.Title = "ID: " + uniqueID;
                    chooseRecipe(window, uniqueID);
                }
            }
        }
    }
}