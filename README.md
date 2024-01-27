# Recipe Radar Application

This project is a C# based recipe curation application that allows you to find, curate and save recipes visualised with a WPF GUI, using the Spoonacular API to fetch recipes. Contains API integration, responsive WPF interface, SQL Backend database to store user and recipe data, dynamic parameterised recipe search and selection and application specific styling on WPF content.
## Application Overview

__Search for specialised recipes with a robust parameterised search__

![](https://github.com/oliciep/Recipe_Radar/blob/main/demos/searchGif.gif) 

__Select from a variety of recipes, and then access more information about any recipe__

![](https://github.com/oliciep/Recipe_Radar/blob/main/demos/infoGif.gif) 

__Sign up and log in to store and access your saved recipes__

![](https://github.com/oliciep/Recipe_Radar/blob/main/demos/loginGif.gif) 


## Features

- C# Based backend to implement recipe finding logic.
- Uses Spoonacular API to fetch recipes based on parameters.
- WPF GUI using buttons, general styling and displaying information.
- SQL based registration system for log in and sign up
- SQL database integration for storing recipe information and users' recipes.
- Filtering system to hone in on specialised recipes.
- Advanced recipe select hub with further recipe information.
- Console application version available for efficiency.

## Deployment

- Ensure you have .NET SDK installed.
- Add API Config File (_APIKeysLocal.cs_) in *config* directory.

_APIKeysLocal.cs_

```bash
public static partial class APIKeys
{
    static APIKeys()
    {
        SpoonacularKey = "ADD KEY HERE";
     }
}
```


## Acknowledgements

 - [Spoonacular API](https://spoonacular.com/food-api)
 - [Favicon Image by _Those Icons_](https://www.flaticon.com/free-icon/chef_483841?term=chef+hat&page=1&position=6&origin=search&related_id=483841)


## Authors

- [@oliciep](https://www.github.com/oliciep)

