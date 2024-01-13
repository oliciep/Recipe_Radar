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
using System.Xml;
using System.Text.RegularExpressions;
using Recipe_Radar.dbConfig;

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string cuisineType = "";
        private int numberOfRecipes = 1;
        private int maxTimeAllowed = 60;
        private bool isDialogShown = false;
        private RootObject? fetchedRecipes;

        public MainWindow()
        {

            InitializeComponent();

            FindButton.Click += FindButton_Click;
            RecipesComboBox.SelectedIndex = 0;
            using (var context = new YourDbContext())
            {

                List<User> users = context.ListUsers();
                foreach (var user in users)
                {
                    Console.WriteLine($"UserID: {user.UserID}, Username: {user.Username}, Password: {user.Password}");
                }
            }
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
                HttpResponseMessage response = await client.GetAsync($"recipes/complexSearch?query={ingredients}&cuisine={cuisineType}&maxReadyTime={maxTimeAllowed}&number={numberOfRecipes}&apiKey={apiKey}");

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

        private void createLoginWindow(object sender, RoutedEventArgs e)
        {
            var stackPanel = new StackPanel();


            var loginWindow = new Window();
            loginWindow.Title = "Log In";
            loginWindow.Width = 400;
            loginWindow.Height = 300;
            loginWindow.Background = Brushes.LightGreen;

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Log ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55")) });
            titleBlock.Inlines.Add(new Run("In") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137")) });
            titleBlock.FontFamily = new FontFamily("Impact");
            titleBlock.FontSize = 48;

            titleBlock.Foreground = Brushes.DarkOliveGreen;
            titleBlock.Margin = new Thickness(10);
            titleBlock.TextAlignment = TextAlignment.Center;

            stackPanel.Children.Add(titleBlock);
            loginWindow.Content = stackPanel;
            loginWindow.ShowDialog();
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

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Your ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55")) });
            titleBlock.Inlines.Add(new Run("Recipes") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137")) });
            titleBlock.FontFamily = new FontFamily("Impact");
            titleBlock.FontSize = 36;
            titleBlock.TextAlignment = TextAlignment.Center;

            stackPanel.Children.Add(titleBlock);

            foreach (var recipe in rootObject.Results)
            {
                Border dividerLine = new Border();
                dividerLine.Width = 700;
                dividerLine.Height = 2;
                dividerLine.Background = CreateFadingBrush();
                dividerLine.Margin = new Thickness(0, 5, 0, 5);

                TextBlock textBlock = new TextBlock();
                textBlock.Inlines.Add(new Run("Recipe: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
                textBlock.Inlines.Add(new Run($"{recipe.Title}") { Foreground = Brushes.Olive });
                textBlock.FontSize = 18;
                textBlock.Margin = new Thickness(10);
                textBlock.TextAlignment = TextAlignment.Center;

                BitmapImage bitmap = new BitmapImage(new Uri(recipe.Image));
                Image img = new Image();
                img.Source = bitmap;
                img.Width = 300;
                img.Height = 200;
                img.Margin = new Thickness(10);

                Button chooseButton = new Button();
                chooseButton.Content = $"Choose Recipe";
                chooseButton.Style = (Style)Resources["ButtonStyle"];
                chooseButton.Tag = recipe.Id;
                chooseButton.Click += chooseButton_Click;

                stackPanel.Children.Add(dividerLine);
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

        private LinearGradientBrush CreateFadingBrush()
        {
            LinearGradientBrush gradientBrush = new LinearGradientBrush();

            gradientBrush.StartPoint = new Point(0, 0.5);
            gradientBrush.EndPoint = new Point(1, 0.5);

            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 86, 202, 85), 0.2));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 86, 202, 85), 0.8));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));

            return gradientBrush;
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

            StringBuilder ingredientsList = new StringBuilder();
            List<string> uniqueIngredients = new List<string>();
            foreach (var ingredient in recipeInformation.ExtendedIngredients)
            {
                if (!uniqueIngredients.Contains($"• {ingredient.Amount} {ingredient.Unit} of {ingredient.Name}\n"))
                {
                    uniqueIngredients.Add($"• {ingredient.Amount} {ingredient.Unit} of {ingredient.Name}\n");
                }
            }

            foreach (var ingredient in uniqueIngredients)
            {
                ingredientsList.Append(ingredient);
            }


            StringBuilder instructionsList = new StringBuilder();
            string filterInstructions = Regex.Replace(recipeInformation.Instructions.ToString(), "<.*?>", "");
            string[] instructions = filterInstructions.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            const int maxLength = 130;
            const int ninthInstruction = 9;

            for (int i = 0; i < instructions.Length; i++)
            {
                string trimmedInstruction = instructions[i].Trim();

                if (trimmedInstruction.Length > maxLength)
                {
                    int index = maxLength;
                    int currentInstruction = i + 1;

                    while (index < trimmedInstruction.Length)
                    {
                        while (index > 0 && trimmedInstruction[index] != ' ')
                        {
                            index--;
                        }

                        if (index == 0)
                        {
                            index = maxLength;
                        }

                        int spacesToAdd = (currentInstruction > ninthInstruction) ? 5 : 3;
                        string spaces = new string(' ', spacesToAdd);

                        trimmedInstruction = trimmedInstruction.Insert(index, Environment.NewLine + spaces);
                        index += maxLength + Environment.NewLine.Length + spacesToAdd;

                        currentInstruction++;
                    }
                }

                instructionsList.Append((i + 1) + ": " + trimmedInstruction + "\n");
            }


            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            var stackPanel = new StackPanel();

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Recipe: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
            titleBlock.Inlines.Add(new Run($"{recipeInformation.Title}") { Foreground = Brushes.Olive });
            titleBlock.FontSize = 24;
            titleBlock.Margin = new Thickness(10);
            titleBlock.TextAlignment = TextAlignment.Center;

            Border dividerLine = new Border();
            dividerLine.Width = 700;
            dividerLine.Height = 2;
            dividerLine.Background = Brushes.Transparent;
            dividerLine.Margin = new Thickness(0, 5, 0, 5);

            BitmapImage bitmap = new BitmapImage(new Uri(recipeInformation.Image));
            Image recipeImage = new Image();
            recipeImage.Source = bitmap;
            recipeImage.Width = 400;
            recipeImage.Height = 300;
            recipeImage.Margin = new Thickness(10);
            recipeImage.VerticalAlignment = VerticalAlignment.Top;

            TextBlock ingredientsTitleBlock = new TextBlock();
            ingredientsTitleBlock.Text = "             Ingredients";
            ingredientsTitleBlock.FontSize = 24;
            ingredientsTitleBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21"));
            ingredientsTitleBlock.TextAlignment = TextAlignment.Center;
            ingredientsTitleBlock.VerticalAlignment = VerticalAlignment.Top;

            TextBlock ingredientsBlock = new TextBlock();
            ingredientsBlock.Text = ingredientsList.ToString();
            ingredientsBlock.FontSize = 12;
            ingredientsBlock.Foreground = Brushes.DarkOliveGreen;
            ingredientsBlock.Margin = new Thickness(10);
            ingredientsBlock.TextAlignment = TextAlignment.Left;
            ingredientsBlock.VerticalAlignment = VerticalAlignment.Top;

            Border ingredientsBorder = new Border();
            ingredientsBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
            ingredientsBorder.Width = 750;
            ingredientsBorder.CornerRadius = new CornerRadius(10);

            Border dividerLine2 = new Border();
            dividerLine2.Width = 700;
            dividerLine2.Height = 20;
            dividerLine2.Background = Brushes.Transparent;
            dividerLine2.Margin = new Thickness(0, 5, 0, 5);

            TextBlock instructionsTitleBlock = new TextBlock();
            instructionsTitleBlock.Text = "  Instructions";
            instructionsTitleBlock.FontSize = 24;
            instructionsTitleBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21"));
            ingredientsBlock.Margin = new Thickness(10);
            instructionsTitleBlock.TextAlignment = TextAlignment.Left;
            instructionsTitleBlock.VerticalAlignment = VerticalAlignment.Top;

            TextBlock instructionsBlock = new TextBlock();
            instructionsBlock.Text = instructionsList.ToString();
            instructionsBlock.FontSize = 12;
            instructionsBlock.Foreground = Brushes.DarkOliveGreen;
            instructionsBlock.Margin = new Thickness(10);
            instructionsBlock.TextAlignment = TextAlignment.Left;

            Border instructionsBorder = new Border();
            instructionsBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#67cf66"));
            instructionsBorder.Width = 750;
            instructionsBorder.CornerRadius = new CornerRadius(10);

            TextBlock infoBlock = new TextBlock();
            infoBlock.Inlines.Add(new Run("Ready in: ") { FontStyle = FontStyles.Italic });
            infoBlock.Inlines.Add(new Run($"{recipeInformation.ReadyInMinutes} minutes. \n") { FontWeight = FontWeights.Bold });
            infoBlock.Inlines.Add(new Run("Serves: ") { FontStyle = FontStyles.Italic });
            infoBlock.Inlines.Add(new Run($"{recipeInformation.Servings} people.") { FontWeight = FontWeights.Bold });
            infoBlock.FontSize = 18;
            infoBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21"));
            infoBlock.Margin = new Thickness(10);
            infoBlock.TextAlignment = TextAlignment.Center;

            Button returnButton = new Button();
            returnButton.Content = "Return to Recipe Select";
            returnButton.Style = (Style)Resources["ButtonStyle"];
            returnButton.Tag = window;
            returnButton.Click += returnButton_Click;

            var ingredientsTextPanel = new StackPanel();
            ingredientsTextPanel.Orientation = Orientation.Vertical;
            ingredientsTextPanel.Children.Add(ingredientsTitleBlock);
            ingredientsTextPanel.Children.Add(ingredientsBlock);

            var ingredientsPanel = new StackPanel();
            ingredientsPanel.Orientation = Orientation.Horizontal;
            ingredientsPanel.Width = 700;
            ingredientsPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
            ingredientsPanel.Children.Add(recipeImage);
            ingredientsPanel.Children.Add(ingredientsTextPanel);
            ingredientsBorder.Child = ingredientsPanel;

            var instructionsPanel = new StackPanel();
            instructionsPanel.Orientation = Orientation.Vertical;
            instructionsPanel.Children.Add(instructionsTitleBlock);
            instructionsPanel.Children.Add(instructionsBlock);
            instructionsBorder.Child = instructionsPanel;

            stackPanel.Children.Add(titleBlock);
            stackPanel.Children.Add(dividerLine);
            stackPanel.Children.Add(ingredientsBorder);
            stackPanel.Children.Add(dividerLine2);
            stackPanel.Children.Add(instructionsBorder);
            stackPanel.Children.Add(infoBlock);
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
        private void CuisinesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CuisinesComboBox.SelectedItem != null)
            {
                cuisineType = ((ComboBoxItem)CuisinesComboBox.SelectedItem).Content.ToString();
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