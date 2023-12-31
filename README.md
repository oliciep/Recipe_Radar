# Recipe Radar Application

This project is a C# based recipe application that allows you to find recipes, visualised with a WPF GUI, using the Spoonacular API to fetch recipes. Contains API integration, responsive WPF interface, dynamic recipe search, multiple parameter tuning for accurate results and styling on WPF content.
## Demo


![](https://github.com/oliciep/Recipe_Radar/blob/main/demo.gif) 


## Features

- C# Based backend to implement recipe finding logic.
- Uses Spoonacular API to fetch recipes based on parameters.
- WPF GUI using buttons, inputs and displaying information.
- WPF styling for visually-pleasing design.
- Filtering system to hone in on user needs.
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


## Authors

- [@oliciep](https://www.github.com/oliciep)

