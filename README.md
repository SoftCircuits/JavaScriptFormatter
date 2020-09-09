<a href="https://www.buymeacoffee.com/jonathanwood" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/black_img.png" alt="Buy Me A Coffee" style="height: 37px !important;width: 170px !important;" ></a>

# JavaScriptFormatter

[![NuGet version (SoftCircuits.JavaScriptFormatter)](https://img.shields.io/nuget/v/SoftCircuits.JavaScriptFormatter.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.JavaScriptFormatter/)

```
Install-Package SoftCircuits.JavaScriptFormatter
```

## Introduction

SoftCircuits.JavaScriptFormatter is a JavaScript formatter library for .NET. It may be particularly useful for converting minified JavaScript to make it more readable.

## Usage

To format a JavaScript string, declare an instance of the `JavaScriptFormatter` class and call the `Format()` method.

```cs
JavaScriptFormatter formatter = new JavaScriptFormatter();
string result = formatter.Format(javascript);
```

To customize the behavior of this class, the constructor can optionally accept an instance of the `FormatOptions` class. This class contains the following members.

| Member | Meaning |
|---|---|
| `string Tab` | Specifies the string used for each indentation. Set to 4 spaces by default. |
| `bool NewLineBetweenFunctions` | Specifies if an empty line is inserted between functions. Set to true by default. |
| `bool OpenBraceOnNewLine` | Specifies if opening braces go on a new line. Set to false by default. |
| `bool NewLineBeforeLineComment` | Specifies if line comments go on new line. Set to true by default. |
| `bool NewLineBeforeInlineComment` | Specifies if inline comments go on a new line. Set to true by default. |
| `bool NewLineAfterInlineComment` | Specifies if a new line should follow inline comments. Set to true by default |
