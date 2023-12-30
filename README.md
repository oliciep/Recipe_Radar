# Recipe Radar Application

This project is a C# based recipe application that allows you to find recipes, using the Spoonacular API to fetch recipes. Contains API integration, dynamic recipe search, responsive user interface and more.
## Features

- C# Based backend to implement recipe search.
- Uses Spoonacular API to fetch recipes based on search.
- Filtering system to hone in on user needs.
- User Interface allowing for multiple user choices.

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