# EnvironmentVariables

[![NuGet version](https://badge.fury.io/nu/EnvironmentVariables.svg)](https://badge.fury.io/nu/EnvironmentVariables)

Small .NET Standard library to easy access to all environment variables you need.\
Targets .NET Standard 2.1 (.NET Core 3.x)

# Installation
Install with NuGet Package Manager Console

```
Install-Package EnvironmentVariables
```
Install with .NET CLI

```
dotnet add package EnvironmentVariables
```

# Usage
## Step 1: Create class that describes all variables you need to access (POCO)
Use public properties, not fields. Add `Env` attribute with variable name to every property you want to get from environment.

```csharp
using EnvironmentVariables;

public class EnvConfig
{
    [Env("ASPNETCORE_ENVIRONMENT")]
    public string AspNetCoreEnvironment { get; set; }

    [Env("MY_ENV_INT")]
    public int MyEnvInt { get; set; }

    [Env("MY_ENV_BOOL")]
    public bool MyEnvBool { get; set; }

    [Env("MY_ENV_STRING_ARRAY")]
    public string[] MyEnvStringArray { get; set; }

    [Env("MY_ENV_INT_DICT")]
    public Dictionary<int, string> MyEnvIntStrDictionary { get; set; }
}
```

## Step 2: Create provider to access your variables
Create new `EnvironmentProvider` with class your crated earlier as a type parameter.
```csharp
using EnvironmentVariables;

var provider = new EnvironmentProvider<EnvConfig>();
```
Access variables with `Values` property
```csharp
Console.WriteLine(provider.Values.AspNetCoreEnvironment);
//Development

Console.WriteLine(provider.Values.MyEnvInt);
//123

Console.WriteLine(provider.Values.MyEnvBool);
//True
```

# Type support

This library supports: 
- `string`
- all built-in value types (`bool`, `int`, `long`, `float`, `double`, `decimal`, etc.)
- collections (`IEnumerable`, `Array`, `List`, etc.)
- `Dictionary`
- `Enum`
- `Nullable<>`

You can check `Tests` project to make sure that particular type is supported.

## Collections

For collections in your environment variables use `,` or `;` as delimiter:
```
one,two,three

one;two;three
```

## Dictionaries

For dictionaries in your environment variables use `=` or `:` as delimiter between key and value:
```
firstkey=123;secondkey=321;thirdkey=0

1:true, 2:false, 3:true
```
Converter ignores spaces so feel free to use it anywhere you want.

# Other features
You may specify the function that will be used to access  variables values
```csharp
var provider = new EnvironmentProvider<EnvConfig>() {
    EnvProvider = name => ThisIsMyFunctionToAccessTheValues(name)
};
```
Use selflog for debugging
```csharp
var provider = new EnvironmentProvider<EnvConfig>() {
    SelfLog = log => Console.WriteLine(log)
};
```
Use `Reload` to reload values from environment
```csharp
provider.Reload();
```

# License
[MIT](https://raw.githubusercontent.com/victortrusov/EnvironmentVariables/master/LICENSE)
