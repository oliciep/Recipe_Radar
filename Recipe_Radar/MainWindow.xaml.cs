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
using Recipe_Radar.dbConfig;
using Microsoft.EntityFrameworkCore;
using System.Windows.Media.Effects;

namespace RecipeRadar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Private variables for account information handling
        private Window loginWindow;
        private Window registerWindow;
        private Window accountWindow;
        private TextBox usernameBox;
        private TextBox passwordBox;
        private Boolean logged_in = false;
        private int user_id;

        // Private variables for general recipe handling
        private string cuisineType = "";
        private int numberOfRecipes = 1;
        private int maxTimeAllowed = 60;
        private bool isDialogShown = false;
        private RootObject? fetchedRecipes;


        /// <summary>
        /// Initialises main window, initialises XAML elements
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            AccountPanel.Visibility = Visibility.Collapsed;
            FindButton.Click += FindButton_Click;
            RecipesComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// [Register Window] Logic for creating account registering window
        /// </summary>
        private void createRegisterWindow(object sender, RoutedEventArgs e)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 10, 0, 10);

            registerWindow = new Window();
            registerWindow.Title = "Register";
            registerWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/logo.ico"));
            registerWindow.Width = 400;
            registerWindow.Height = 300;
            registerWindow.Background = Brushes.LightGreen;

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Register") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137")) });
            titleBlock.FontFamily = new FontFamily("Impact");
            titleBlock.FontSize = 48;
            titleBlock.Foreground = Brushes.DarkOliveGreen;
            titleBlock.Margin = new Thickness(10);
            titleBlock.TextAlignment = TextAlignment.Center;

            TextBlock usernameBlock = new TextBlock();
            usernameBlock.Text = "Username:   ";
            usernameBlock.FontSize = 16;
            usernameBlock.FontWeight = FontWeights.Bold;
            usernameBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137"));
            usernameBlock.TextAlignment = TextAlignment.Left;

            usernameBox = new TextBox();
            ApplyRoundedCorners(usernameBox);
            usernameBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC6EAA2"));
            usernameBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F5318"));
            usernameBox.HorizontalAlignment = HorizontalAlignment.Right;
            usernameBox.Width = 250;

            TextBlock passwordBlock = new TextBlock();
            passwordBlock.Text = "Password:    ";
            passwordBlock.FontSize = 16;
            passwordBlock.FontWeight = FontWeights.Bold;
            passwordBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137"));
            passwordBlock.TextAlignment = TextAlignment.Left;

            passwordBox = new TextBox();
            ApplyRoundedCorners(passwordBox);
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC6EAA2"));
            passwordBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F5318"));
            passwordBox.HorizontalAlignment = HorizontalAlignment.Right;
            passwordBox.Width = 250;

            var usernamePanel = new StackPanel();
            usernamePanel.Children.Add(usernameBlock);
            usernamePanel.Children.Add(usernameBox);
            usernamePanel.Orientation = Orientation.Horizontal;
            usernamePanel.Margin = new Thickness(10);

            var passwordPanel = new StackPanel();
            passwordPanel.Children.Add(passwordBlock);
            passwordPanel.Children.Add(passwordBox);
            passwordPanel.Orientation = Orientation.Horizontal;
            passwordPanel.Margin = new Thickness(10);

            Button registerButton = new Button();
            registerButton.Content = $"Register";
            registerButton.Style = (Style)Resources["ButtonStyle"];
            registerButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55"));
            registerButton.Height = 40;
            registerButton.Width = 100;
            registerButton.Click += RegisterButton_Click;

            DataContext = this;

            stackPanel.Children.Add(usernamePanel);
            stackPanel.Children.Add(passwordPanel);
            stackPanel.Children.Add(registerButton);

            Border registerPanelBorder = new Border();
            registerPanelBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
            registerPanelBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52922a"));
            registerPanelBorder.BorderThickness = new Thickness(2);
            registerPanelBorder.Width = 370;
            registerPanelBorder.CornerRadius = new CornerRadius(10);
            registerPanelBorder.Child = stackPanel;

            var registerPanel = new StackPanel();
            registerPanel.Children.Add(titleBlock);
            registerPanel.Children.Add(registerPanelBorder);

            registerWindow.Content = registerPanel;
            registerWindow.ShowDialog();
        }

        /// <summary>
        /// [Register Window] Logic for register button press
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Check for null inputs in text boxes
            if (usernameBox != null && passwordBox != null)
            {
                string username = usernameBox.Text;
                string password = passwordBox.Text;
                registerUser(username, password);
            }
        }

        /// <summary>
        /// [Register Window] Logic for authenticating valid user sign up
        /// </summary>
        private void registerUser(string username, string password)
        {
            Boolean valid = true;
            using (var context = new YourDbContext())
            {
                // Iterates through list to check if username is already taken
                List<User> users = context.ListUsers();
                foreach (var user in users)
                {
                    if (user.Username == username)
                    {
                        MessageBox.Show($"Name is already taken.");
                        valid = false;
                        break;
                    }
                }
            }

            // Checks for invalid password length (less than 8 characters)
            if (password.Length < 8)
            {
                MessageBox.Show("Password must be 8 or more characters.");
                valid = false;
            }

            // If all checks are valid user is added to users table
            if (valid)
            {
                MessageBox.Show("Successfully signed up.");
                using (var context = new YourDbContext())
                {
                    registerWindow.Close();
                    var newUser = new User();
                    newUser.Username = username;
                    newUser.Password = password;
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// [Login Window] Logic for creating account login window
        /// </summary>
        private void createLoginWindow(object sender, RoutedEventArgs e)
        {
            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(0, 10, 0, 10);

            loginWindow = new Window();
            loginWindow.Title = "Log In";
            loginWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/logo.ico"));
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

            TextBlock usernameBlock = new TextBlock();
            usernameBlock.Text = "Username:   ";
            usernameBlock.FontSize = 16;
            usernameBlock.FontWeight = FontWeights.Bold;
            usernameBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137"));
            usernameBlock.TextAlignment = TextAlignment.Left;

            usernameBox = new TextBox();
            ApplyRoundedCorners(usernameBox);
            usernameBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC6EAA2"));
            usernameBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F5318"));
            usernameBox.HorizontalAlignment = HorizontalAlignment.Right;
            usernameBox.Width = 250;

            TextBlock passwordBlock = new TextBlock();
            passwordBlock.Text = "Password:    ";
            passwordBlock.FontSize = 16;
            passwordBlock.FontWeight = FontWeights.Bold;
            passwordBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137"));
            passwordBlock.TextAlignment = TextAlignment.Left;

            passwordBox = new TextBox();
            ApplyRoundedCorners(passwordBox);
            passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC6EAA2"));
            passwordBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F5318"));
            passwordBox.HorizontalAlignment = HorizontalAlignment.Right;
            passwordBox.Width = 250;

            var usernamePanel = new StackPanel();
            usernamePanel.Children.Add(usernameBlock);
            usernamePanel.Children.Add(usernameBox);
            usernamePanel.Orientation = Orientation.Horizontal;
            usernamePanel.Margin = new Thickness(10);

            var passwordPanel = new StackPanel();
            passwordPanel.Children.Add(passwordBlock);
            passwordPanel.Children.Add(passwordBox);
            passwordPanel.Orientation = Orientation.Horizontal;
            passwordPanel.Margin = new Thickness(10);

            Button loginButton = new Button();
            loginButton.Content = $"Log In";
            loginButton.Style = (Style)Resources["ButtonStyle"];
            loginButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55"));
            loginButton.Height = 40;
            loginButton.Width = 100;
            loginButton.Click += LoginButton_Click;

            DataContext = this;

            stackPanel.Children.Add(usernamePanel);
            stackPanel.Children.Add(passwordPanel);
            stackPanel.Children.Add(loginButton);

            Border loginPanelBorder = new Border();
            loginPanelBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
            loginPanelBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52922a"));
            loginPanelBorder.BorderThickness = new Thickness(2);
            loginPanelBorder.Width = 370;
            loginPanelBorder.CornerRadius = new CornerRadius(10);
            loginPanelBorder.Child = stackPanel;

            var loginPanel = new StackPanel();
            loginPanel.Children.Add(titleBlock);
            loginPanel.Children.Add(loginPanelBorder);

            loginWindow.Content = loginPanel;
            loginWindow.ShowDialog();
        }

        /// <summary>
        /// [Login Window] Logic for login button press
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Checks for null inputs in text boxes
            if (usernameBox != null && passwordBox != null)
            {
                string username = usernameBox.Text;
                string password = passwordBox.Text;

                AuthenticateUser(username, password);
            }
        }

        /// <summary>
        /// [Login Window] Logic for authenticating valid user log in
        /// </summary>
        private void AuthenticateUser(string username, string password)
        {
            bool success = false;
            using (var context = new YourDbContext())
            {
                List<User> users = context.ListUsers();
                foreach (var user in users)
                {
                    // If the username and password match, then user is logged in
                    if (user.Username == username && user.Password == password)
                    {
                        ButtonsPanel.Visibility = Visibility.Collapsed;
                        // Log in and register buttons are replaced by account buttons
                        AccountPanel.Visibility = Visibility.Visible;
                        logged_in = true;
                        user_id = user.UserID;
                        loginWindow.Close();
                        createHomepageWindow(user.UserID, username, false);
                        success = true;
                        break;
                    }
                }
            }
            if (!success)
            {
                MessageBox.Show("Login unsuccessful.");
            }
        }

        /// <summary>
        /// [Account Window] Logic for sending user to account page on button press
        /// </summary>
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            using (YourDbContext context = new YourDbContext())
            {
                User? user = context.Users.FirstOrDefault(u => u.UserID == user_id);
                createHomepageWindow(user_id, user.Username, false);
            }
        }

        /// <summary>
        /// [Account Window] Logic for creating home page window 
        /// </summary>
        private void createHomepageWindow(int ID, string username, bool returned)
        {
            // If user has already created the window, ignores repeat creation
            if (!returned)
            {
                accountWindow = new Window();
            }
            accountWindow.Title = $"{username}'s homepage";
            accountWindow.Name = "AccountSearch";
            accountWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/logo.ico"));
            accountWindow.Width = 800;
            accountWindow.Height = 600;
            accountWindow.Background = Brushes.LightGreen;
            fetchAccountInfo(ID, username, accountWindow, returned);
        }

        /// <summary>
        /// [Account Window] Logic for fetching users' information to display on account window
        /// </summary>
        private void fetchAccountInfo(int ID, string username, Window accountWindow, bool returned)
        {
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = null;

            var stackPanel = new StackPanel();

            Button logOutButton = new Button();
            logOutButton.Content = "Log out";
            logOutButton.Style = (Style)Resources["ButtonStyle"];
            logOutButton.Click += logOutButton_Click;
            logOutButton.HorizontalAlignment = HorizontalAlignment.Right;
            logOutButton.Margin = new Thickness(0, 5, 5, 20);
            stackPanel.Children.Add(logOutButton);

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Welcome, ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55")) });
            titleBlock.Inlines.Add(new Run($"{username}.") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137")) });
            titleBlock.FontFamily = new FontFamily("Impact");
            titleBlock.FontSize = 48;
            titleBlock.Margin = new Thickness(0, 0, 0, 10);
            titleBlock.TextAlignment = TextAlignment.Center;

            Border titleBorder = new Border();
            titleBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
            titleBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#50c84e"));
            titleBorder.BorderThickness = new Thickness(2);
            titleBorder.CornerRadius = new CornerRadius(10);
            titleBorder.Width = 600;
            titleBorder.Child = titleBlock;
            stackPanel.Children.Add(titleBorder);

            TextBlock recipeTitleBlock = new TextBlock();
            recipeTitleBlock.Inlines.Add(new Run("Your ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56ca55")) });
            recipeTitleBlock.Inlines.Add(new Run("Recipes") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38b137")) });
            recipeTitleBlock.FontFamily = new FontFamily("Impact");
            recipeTitleBlock.FontSize = 36;
            recipeTitleBlock.Margin = new Thickness(20, 10, 0, 0);
            recipeTitleBlock.TextAlignment = TextAlignment.Left;
            stackPanel.Children.Add(recipeTitleBlock);

            using (var context = new YourDbContext())
            {
                // SQL Query that will check every recipe that the user is linked to and list them
                var recipesForUser = context.UserRecipes
                    .Where(ur => ur.UserID == user_id)
                    .Include(ur => ur.Recipe)
                    .ThenInclude(recipe => recipe.Ingredients)
                    .Select(ur => ur.Recipe)
                    .ToList();
                foreach (var recipe in recipesForUser)
                {
                    StackPanel recipePanel = new StackPanel();
                    StackPanel recipeButtonsPanel = new StackPanel();

                    BitmapImage bitmap = new BitmapImage(new Uri(recipe.Image));
                    Image img = new Image();
                    img.Source = bitmap;
                    img.Width = 300;
                    img.Height = 200;
                    img.Margin = new Thickness(5, 10, 10, 10);
                    recipePanel.Children.Add(img);

                    TextBlock recipeBlock = new TextBlock();
                    recipeBlock.Inlines.Add(new Run("Recipe: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
                    recipeBlock.Inlines.Add(new Run($"{recipe.Title}") { Foreground = Brushes.Olive });
                    recipeBlock.FontSize = 20;
                    recipeBlock.Margin = new Thickness(10, 5, 0, 5);
                    recipeBlock.TextAlignment = TextAlignment.Left;
                    recipeBlock.TextWrapping = TextWrapping.Wrap;
                    recipeBlock.MaxWidth = 420;

                    TextBlock recipeInfoBlock = new TextBlock();
                    recipeInfoBlock.Inlines.Add(new Run("Serves: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
                    recipeInfoBlock.Inlines.Add(new Run($"{recipe.Servings} people\n") { Foreground = Brushes.Olive });
                    recipeInfoBlock.Inlines.Add(new Run("Ready in: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
                    recipeInfoBlock.Inlines.Add(new Run($"{recipe.ReadyTime} minutes") { Foreground = Brushes.Olive });
                    recipeInfoBlock.FontSize = 16;
                    recipeInfoBlock.Margin = new Thickness(10, 0, 0, 10);
                    recipeInfoBlock.TextAlignment = TextAlignment.Left;
                    recipeInfoBlock.VerticalAlignment = VerticalAlignment.Top;

                    Button removeRecipeButton = new Button();
                    removeRecipeButton.Content = $"Remove Recipe";
                    removeRecipeButton.Style = (Style)Resources["ButtonStyle"];
                    removeRecipeButton.Tag = recipe.RecipeID;
                    removeRecipeButton.Click += removeRecipeButton_Click;
                    removeRecipeButton.HorizontalAlignment = HorizontalAlignment.Left;
                    recipeButtonsPanel.Children.Add(removeRecipeButton);

                    Button chooseRecipeButton = new Button();
                    chooseRecipeButton.Content = $"Choose Recipe";
                    chooseRecipeButton.Style = (Style)Resources["ButtonStyle"];

                    // Tag is initialised with recreation of JSON object that API returns, allowing for recipe to be selected from saved recipes
                    chooseRecipeButton.Tag = new RecipeInformation
                    {
                        Id = recipe?.RecipeID ?? 0,
                        Title = recipe?.Title,
                        ReadyInMinutes = recipe?.ReadyTime ?? 0,
                        Servings = recipe?.Servings ?? 0,
                        Image = recipe?.Image,
                        Instructions = recipe?.Instructions,
                        ExtendedIngredients = recipe?.Ingredients
                            ?.Select(ingredient => new ExtendedIngredient
                            {
                                Name = ingredient?.Name,
                                Amount = ingredient?.Amount ?? 0,
                                Unit = ingredient?.Unit
                            })
                            .ToList() ?? new List<ExtendedIngredient>()
                    };
                    chooseRecipeButton.Click += chooseRecipeButton_Click;
                    chooseRecipeButton.HorizontalAlignment = HorizontalAlignment.Right;
                    recipeButtonsPanel.VerticalAlignment = VerticalAlignment.Bottom;

                    recipeButtonsPanel.Children.Add(chooseRecipeButton);
                    recipeButtonsPanel.Orientation = Orientation.Horizontal;
                    recipeButtonsPanel.Margin = new Thickness(10, 0, 0, 0);

                    var recipeInfoPanel = new Grid();
                    RowDefinition titleRow = new RowDefinition();
                    RowDefinition buttonsRow = new RowDefinition();

                    recipeInfoPanel.RowDefinitions.Add(titleRow);
                    recipeInfoPanel.RowDefinitions.Add(buttonsRow);

                    StackPanel textStackPanel = new StackPanel();
                    textStackPanel.Children.Add(recipeBlock);
                    textStackPanel.Children.Add(recipeInfoBlock);

                    Grid.SetRow(textStackPanel, 0);
                    Grid.SetRow(recipeButtonsPanel, 1);

                    recipeInfoPanel.Children.Add(textStackPanel);
                    recipeInfoPanel.Children.Add(recipeButtonsPanel);

                    Border recipeInfoBorder = new Border();
                    recipeInfoBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
                    recipeInfoBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#50c84e"));
                    recipeInfoBorder.BorderThickness = new Thickness(2);
                    recipeInfoBorder.Width = 400;
                    recipeInfoBorder.Margin = new Thickness(10, 10, 0, 10);
                    recipeInfoBorder.CornerRadius = new CornerRadius(10);
                    recipeInfoBorder.Child = recipeInfoPanel;

                    Border recipeBorder = new Border();
                    recipeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
                    recipeBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52922a"));
                    recipeBorder.BorderThickness = new Thickness(2);
                    recipeBorder.Margin = new Thickness(10, 10, 0, 10);
                    recipeBorder.Width = 750;
                    recipeBorder.CornerRadius = new CornerRadius(10);

                    recipePanel.Children.Add(recipeInfoBorder);
                    recipePanel.Orientation = Orientation.Horizontal;
                    recipePanel.Width = 730;

                    recipeBorder.Child = recipePanel;
                    stackPanel.Children.Add(recipeBorder);
                }
            }
            scrollViewer.Content = stackPanel;
            accountWindow.Content = scrollViewer;
            if (!returned)
            {
                accountWindow.ShowDialog();
            }
        }

        /// <summary>
        /// [Account Window] Logic for removing recipe from users' account on button press
        /// </summary>
        private void removeRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            int recipe_id = Convert.ToInt32(button.Tag);
            try
            {
                using (var context = new YourDbContext())
                {
                    // SQL code to remove recipe alongside any linked properties it has
                    context.RemoveRecipe(user_id, recipe_id);
                    MessageBox.Show("Recipe removed successfully.");

                    var user = context.Users.FirstOrDefault(u => u.UserID == user_id);
                    // Refreshes the account window
                    createHomepageWindow(user_id, user.Username, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing recipe: {ex.Message}");
            }
        }

        /// <summary>
        /// [Account Window] Logic for displaying saved recipe on button press
        /// </summary>
        private void chooseRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            RecipeInformation recipeInfo = (RecipeInformation)clickedButton.Tag;
            fetchInfo(accountWindow, recipeInfo);
        }

        /// <summary>
        /// [Account Window/Recipe Information Window] Logic for returning user to previous screen based on button press
        /// </summary>
        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                Window window = Window.GetWindow(button);

                if (window != null)
                {
                    // If the window is for Recipe Information, return to Recipe Select window
                    if (window.Name == "APISearch")
                    {
                        fetchResults(fetchedRecipes, window);
                    }
                    // If the window is for Account Recipe Information, return to Account window
                    else if (window.Name == "AccountSearch")
                    {
                        using (YourDbContext context = new YourDbContext())
                        {
                            User? user = context.Users.FirstOrDefault(u => u.UserID == user_id);
                            createHomepageWindow(user_id, user.Username, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// [Account Window] Logic for logging user out of account on button press
        /// </summary>
        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            logged_in = false;
            user_id = 0;
            accountWindow.Close();
            ButtonsPanel.Visibility = Visibility.Visible;
            AccountPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// [Main Window] Logic for finding recipe based on initial parameters
        /// </summary>
        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder ingredients = new("");
            foreach (var item in ingredientListBox.Items)
            {
                ingredients.Append(item + ", ");
            }

            // Replace file with your API key here
            string? apiKey = APIKeys.SpoonacularKey;
            using (HttpClient client = new HttpClient())
            {
                // Sends query to Spoonacular API with multiple parameters from user inputs
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                HttpResponseMessage response = await client.GetAsync($"recipes/complexSearch?query={ingredients}&cuisine={cuisineType}&maxReadyTime={maxTimeAllowed}&number={numberOfRecipes}&apiKey={apiKey}");

                // Check if JSON is valid
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

            // Test Data object for testing without API request
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

        /// <summary>
        /// [Recipe Select Window] Logic for creating initial recipe select window
        /// </summary>
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

        /// <summary>
        /// [Recipe Select Window] Logic for displaying selectable recipes and styling on recipe select screen
        /// </summary>
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
            titleBlock.Margin = new Thickness(0, 0, 0, 10);
            titleBlock.TextAlignment = TextAlignment.Center;

            stackPanel.Children.Add(titleBlock);

            // Iterates through API request results and returns amount of recipes and their information
            foreach (var recipe in rootObject.Results)
            {
                var recipePanel = new StackPanel();

                Border dividerLine = new Border();
                dividerLine.Width = 700;
                dividerLine.Height = 10;
                dividerLine.Background = Brushes.Transparent;
                dividerLine.Margin = new Thickness(0, 5, 0, 5);

                TextBlock textBlock = new TextBlock();
                textBlock.Inlines.Add(new Run("Recipe: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
                textBlock.Inlines.Add(new Run($"{recipe.Title}") { Foreground = Brushes.Olive });
                textBlock.FontSize = 18;
                textBlock.Margin = new Thickness(0, 5, 0, 10);
                textBlock.TextAlignment = TextAlignment.Left;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.MaxWidth = 450;

                BitmapImage bitmap = new BitmapImage(new Uri(recipe.Image));
                Image img = new Image();
                img.Source = bitmap;
                img.Width = 200;
                img.Height = 150;
                img.Margin = new Thickness(10);

                Button chooseButton = new Button();
                chooseButton.Content = $"Choose Recipe";
                chooseButton.Style = (Style)Resources["ButtonStyle"];
                chooseButton.HorizontalAlignment = HorizontalAlignment.Left;
                chooseButton.VerticalAlignment = VerticalAlignment.Bottom;
                chooseButton.Margin = new Thickness(20, 0, 0, 5);
                chooseButton.Tag = recipe.Id;
                chooseButton.Click += chooseButton_Click;

                Border recipeBorder = new Border();
                recipeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
                recipeBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52922a"));
                recipeBorder.BorderThickness = new Thickness(2);
                recipeBorder.Width = 750;
                recipeBorder.CornerRadius = new CornerRadius(10);

                var recipeInfoPanel = new Grid();
                RowDefinition textRow = new RowDefinition();
                RowDefinition buttonsRow = new RowDefinition();

                Grid.SetRow(recipeInfoPanel, 0);
                Grid.SetRow(chooseButton, 1);


                recipeInfoPanel.Children.Add(textBlock);
                recipeInfoPanel.Children.Add(chooseButton);

                Border titleBorder = new Border();
                titleBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
                titleBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#50c84e"));
                titleBorder.BorderThickness = new Thickness(2);
                titleBorder.CornerRadius = new CornerRadius(10);
                titleBorder.Width = 490;
                titleBorder.Margin = new Thickness(10);
                titleBorder.Child = recipeInfoPanel;

                recipePanel.Children.Add(img);
                recipePanel.Children.Add(titleBorder);
                recipePanel.Orientation = Orientation.Horizontal;

                recipePanel.Width = 730;
                recipeBorder.Child = recipePanel;

                stackPanel.Children.Add(recipeBorder);
                stackPanel.Children.Add(dividerLine);
            }
            
            scrollViewer.Content = stackPanel;

            imageWindow.Title = "Your Recipes";
            imageWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/logo.ico"));
            imageWindow.Name = "APISearch";
            imageWindow.Width = 800;
            imageWindow.Height = 600;
            imageWindow.Background = Brushes.LightGreen;
            imageWindow.Content = scrollViewer;

            // Checks if window has been closed properly or not
            if (!isDialogShown)
            {
                isDialogShown = true;
                imageWindow.ShowDialog();
            }
        }

        /// <summary>
        /// [Recipe Select Window] Logic for selecting certain recipe based on button press
        /// </summary>
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

        /// <summary>
        /// [Recipe Select Window] Logic for selecting a recipe from select screen
        /// </summary>
        private async void chooseRecipe(Window window, int uniqueID)
        {
            // Setting up API request
            string? apiKey = APIKeys.SpoonacularKey;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
                // API Query using Recipe ID to get further information about a recipe
                HttpResponseMessage response = await client.GetAsync($"recipes/{uniqueID}/information?apiKey={apiKey}");

                // Checks if JSON is valid
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

        /// <summary>
        /// [Recipe Information Window] Logic for displaying information about selected recipe
        /// </summary>
        private void fetchInfo(Window window, RecipeInformation recipeInformation)
        {
            window.Title = $"Recipe: {recipeInformation.Title}";

            StringBuilder ingredientsList = new StringBuilder();
            List<string> uniqueIngredients = new List<string>();
            // Adds each ingredient to the string of ingredients list
            foreach (var ingredient in recipeInformation.ExtendedIngredients)
            {
                // Checks if the ingredient has already been listed 
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
            // Regex to filter out any lingering HTML tags
            string filterInstructions = Regex.Replace(recipeInformation.Instructions.ToString(), "<.*?>", "");
            string[] instructions = filterInstructions.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            const int maxLength = 110;
            const int ninthInstruction = 9;

            // Code used to check if instruction is 10th or after. If it is, margin amount is adjusted
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

            Border dividerLine = new Border();
            dividerLine.Width = 700;
            dividerLine.Height = 10;
            dividerLine.Background = Brushes.Transparent;
            dividerLine.Margin = new Thickness(0, 5, 0, 5);

            TextBlock titleBlock = new TextBlock();
            titleBlock.Inlines.Add(new Run("Recipe: ") { Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21")) });
            titleBlock.Inlines.Add(new Run($"{recipeInformation.Title}") { Foreground = Brushes.Olive });
            titleBlock.FontSize = 24;
            titleBlock.Margin = new Thickness(10);
            titleBlock.TextAlignment = TextAlignment.Center;
            titleBlock.TextWrapping = TextWrapping.Wrap;
            titleBlock.Width = 700;

            Border titleBorder = new Border();
            titleBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#93dd92"));
            titleBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#50c84e"));
            titleBorder.BorderThickness = new Thickness(2);
            titleBorder.CornerRadius = new CornerRadius(10);
            titleBorder.Width = 750;
            titleBorder.Child = titleBlock;

            Border dividerLine2 = new Border();
            dividerLine2.Width = 700;
            dividerLine2.Height = 2;
            dividerLine2.Background = Brushes.Transparent;
            dividerLine2.Margin = new Thickness(0, 5, 0, 5);

            BitmapImage bitmap = new BitmapImage(new Uri(recipeInformation.Image));
            Image recipeImage = new Image();
            recipeImage.Source = bitmap;
            recipeImage.Width = 400;
            recipeImage.Height = 300;
            recipeImage.Margin = new Thickness(5, 10, 0, 0);
            recipeImage.VerticalAlignment = VerticalAlignment.Top;

            TextBlock ingredientsTitleBlock = new TextBlock();
            ingredientsTitleBlock.Text = "Ingredients";
            ingredientsTitleBlock.TextDecorations = TextDecorations.Underline;
            ingredientsTitleBlock.FontSize = 24;
            ingredientsTitleBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21"));
            ingredientsTitleBlock.Margin = new Thickness(10, 5, 0, 0);
            ingredientsTitleBlock.TextAlignment = TextAlignment.Left;
            ingredientsTitleBlock.VerticalAlignment = VerticalAlignment.Top;

            DropShadowEffect dropShadowEffect = new DropShadowEffect
            {
                Color = Color.FromRgb(0, 64, 0),
                Opacity = 0.15,
                ShadowDepth = 2,
                BlurRadius = 2
            };

            TextBox ingredientsBlock = new TextBox();
            ingredientsBlock.Text = ingredientsList.ToString();
            ingredientsBlock.FontSize = 12;
            ingredientsBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
            ingredientsBlock.Foreground = Brushes.DarkOliveGreen;
            ingredientsBlock.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
            ingredientsBlock.Margin = new Thickness(10, 5, 0, 0);
            ingredientsBlock.TextAlignment = TextAlignment.Left;
            ingredientsBlock.VerticalAlignment = VerticalAlignment.Top;
            ingredientsBlock.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ingredientsBlock.Width = 270;
            ingredientsBlock.MaxHeight = 240;

            Border ingredientsBorder = new Border();
            ingredientsBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78d577"));
            ingredientsBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#42c441"));
            ingredientsBorder.BorderThickness = new Thickness(2);
            ingredientsBorder.Width = 750;
            ingredientsBorder.CornerRadius = new CornerRadius(10);

            Border dividerLine3 = new Border();
            dividerLine3.Width = 700;
            dividerLine3.Height = 20;
            dividerLine3.Background = Brushes.Transparent;
            dividerLine3.Margin = new Thickness(0, 5, 0, 5);

            TextBlock instructionsTitleBlock = new TextBlock();
            instructionsTitleBlock.Text = "Instructions";
            instructionsTitleBlock.TextDecorations = TextDecorations.Underline;
            instructionsTitleBlock.FontSize = 24;
            instructionsTitleBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#226a21"));
            instructionsTitleBlock.Margin = new Thickness(10, 5, 0, 0);
            instructionsTitleBlock.TextAlignment = TextAlignment.Left;
            instructionsTitleBlock.VerticalAlignment = VerticalAlignment.Top;

            TextBlock instructionsBlock = new TextBlock();
            instructionsBlock.Text = instructionsList.ToString();
            instructionsBlock.FontSize = 14;
            instructionsBlock.Foreground = Brushes.DarkOliveGreen;
            instructionsBlock.Margin = new Thickness(10, 5, 0, 0);
            instructionsBlock.TextAlignment = TextAlignment.Left;
            instructionsBlock.Effect = dropShadowEffect;

            Border instructionsBorder = new Border();
            instructionsBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#67cf66"));
            instructionsBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d8d2c"));
            instructionsBorder.BorderThickness = new Thickness(2);
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

            Border infoBorder = new Border();
            infoBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3cbc3b"));
            infoBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#30962f"));
            infoBorder.BorderThickness = new Thickness(1);
            infoBorder.Width = 200;
            infoBorder.CornerRadius = new CornerRadius(10);
            infoBorder.Child = infoBlock;

            Border dividerLine4 = new Border();
            dividerLine4.Width = 700;
            dividerLine4.Height = 10;
            dividerLine4.Background = Brushes.Transparent;
            dividerLine4.Margin = new Thickness(0, 5, 0, 5);

            Button returnButton = new Button();
            returnButton.Content = "Return to Recipe Select";
            returnButton.Style = (Style)Resources["ButtonStyle"];
            returnButton.Tag = window;
            returnButton.Click += returnButton_Click;

            var buttonsPanel = new StackPanel();
            buttonsPanel.Orientation = Orientation.Horizontal;
            buttonsPanel.HorizontalAlignment = HorizontalAlignment.Center;
            // Checks if user is in recipe select screen
            if (logged_in && window.Name == "APISearch")
            {
                Button saveButton = new Button();
                saveButton.Content = "Save Recipe";
                saveButton.Style = (Style)Resources["ButtonStyle"];
                saveButton.Tag = recipeInformation;
                saveButton.Click += saveButton_Click;
                buttonsPanel.Children.Add(saveButton);
            }
            buttonsPanel.Children.Add(returnButton);

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

            stackPanel.Children.Add(dividerLine);
            stackPanel.Children.Add(titleBorder);
            stackPanel.Children.Add(dividerLine2);
            stackPanel.Children.Add(ingredientsBorder);
            stackPanel.Children.Add(dividerLine3);
            stackPanel.Children.Add(instructionsBorder);
            stackPanel.Children.Add(dividerLine4);
            stackPanel.Children.Add(infoBorder);
            stackPanel.Children.Add(buttonsPanel);

            scrollViewer.Content = stackPanel;
            window.Content = scrollViewer;


        }

        /// <summary>
        /// [Recipe Information Window] Logic for saving recipe to users' account on button press
        /// </summary>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button saveButton && saveButton.Tag is RecipeInformation recipeInformation)
            {
                using (var context = new YourDbContext())
                {
                    // SQL Code to check if that recipe has already been saved to the recipes table
                    bool saved = context.Recipes.Any(recipe => recipe.RecipeID == recipeInformation.Id);
                    // SQL Code to check if that user has saved that recipe to their UserRecipes table
                    bool user_saved = context.Users
                                .Any(u => u.UserRecipes.Any(ur => ur.RecipeID == recipeInformation.Id && ur.UserID == user_id));

                    // If the recipe has not been saved anywhere, code to saved to Recipes, UserRecipes and Ingredients
                    if (!saved)
                    {
                        // Initialising a new record for the recipe
                        var newRecipe = new Recipe_Radar.dbConfig.Recipe
                        {
                            RecipeID = recipeInformation.Id,
                            Title = recipeInformation.Title,
                            ReadyTime = recipeInformation.ReadyInMinutes,
                            Servings = recipeInformation.Servings,
                            Image = recipeInformation.Image,
                            Instructions = recipeInformation.Instructions,
                            Ingredients = recipeInformation.ExtendedIngredients
                                .Select(extendedIngredient => new Ingredient
                                {
                                    RecipeID = recipeInformation.Id,
                                    Name = extendedIngredient.Name,
                                    Amount = extendedIngredient.Amount,
                                    Unit = extendedIngredient.Unit
                                })
                                .ToList(),
                            UserRecipes = new List<UserRecipe>
                        {
                            new UserRecipe
                            {
                                UserID = user_id,
                                RecipeID = recipeInformation.Id
                            }
                        }
                        };
                        context.Recipes.Add(newRecipe);
                        context.SaveChanges();
                        MessageBox.Show("Recipe added to saved recipes globally.");
                    } 
                    // Another check, this time to check if that user has saved the recipe
                    else if (!user_saved)
                    {
                        var user = context.Users.FirstOrDefault(u => u.UserID == user_id);
                        user.UserRecipes.Add(new UserRecipe
                        {
                            RecipeID = recipeInformation.Id,
                            UserID = user_id
                        });

                        context.SaveChanges();
                        MessageBox.Show("Recipe added to saved recipes locally.");
                    } 
                    else
                    {
                        MessageBox.Show("User already saved this recipe.");
                    }
                }
            }
        }

        /// <summary>
        /// [Main Window] Logic for adding ingredient to search on button press
        /// </summary>
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string newIngredient = ingredientsTextBox.Text;
            // Checks if ingredient entered is not null
            if (!string.IsNullOrWhiteSpace(newIngredient))
            {
                ingredientListBox.Items.Add(newIngredient);
                ingredientsTextBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// [Main Window] Logic for removing ingredient from search on button press
        /// </summary>
        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            // Checks if ingredient selected is not null
            if (ingredientListBox.SelectedItem != null)
            {
                ingredientListBox.Items.Remove(ingredientListBox.SelectedItem);
            }
        }

        /// <summary>
        /// [Main Window] Logic for updating variable based on cuisine choice being changed
        /// </summary>
        private void CuisinesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CuisinesComboBox.SelectedItem != null)
            {
                cuisineType = ((ComboBoxItem)CuisinesComboBox.SelectedItem).Content.ToString();
            }
        }


        /// <summary>
        /// [Main Window] Logic for updating variable based on number of recipes choice being changed
        /// </summary>
        private void RecipesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipesComboBox.SelectedItem != null)
            {
                int.TryParse(((ComboBoxItem)RecipesComboBox.SelectedItem).Content.ToString(), out numberOfRecipes);
            }
        }

        /// <summary>
        /// [Main Window] Logic for user inputted max ready time for recipe when text box is clicked off
        /// </summary>
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

        /// <summary>
        /// [General] Logic for applying rounded corners styling to textboxes
        /// </summary>
        private void ApplyRoundedCorners(TextBox textBox)
        {
            // Sets the textbox style to have a corner radius
            Style textBoxStyle = new Style(typeof(TextBox));

            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(5)));

            textBoxStyle.Resources.Add(typeof(Border), borderStyle);

            textBox.Style = textBoxStyle;
        }

        /// <summary>
        /// [General] Logic for ensuring that dialog window is closed
        /// </summary>
        private void ImageWindow_Closed(object sender, EventArgs e)
        {
            isDialogShown = false;
        }

    }
}