# JavaScriptFormatter

[![NuGet version (SoftCircuits.JavaScriptFormatter)](https://img.shields.io/nuget/v/SoftCircuits.JavaScriptFormatter.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.JavaScriptFormatter/)

```
Install-Package SoftCircuits.JavaScriptFormatter
```

## Introduction

SoftCircuits.JavaScriptFormatter is a JavaScript formatter library for .NET. It may be particularly useful for converting minified JavaScript to a more readable format.

## Usage

To use the library, declare an instance of the JavaScriptFormatter and call the `Format()` method.

```cs
JavaScriptFormatter formatter = new JavaScriptFormatter();
string result = formatter.Format(javascript);
```

To customize the behavior of this class, the constructor can optionally accept an instance of the `FormatOptions` class. This class contains the following members.

| Member | Meaning |
|---|---|
| `string Tab` | Specifies the string uses for each indentation. Set to 4 spaces by default. |
| `bool NewLineBetweenFunctions` | Gets or sets if an empty line is inserted between functions. Set to true by default. |
| `bool OpenBraceOnNewLine` | Gets or sets if opening braces go on a new line. Set to false by default. |
| `bool NewLineBeforeLineComment` | Gets or sets if line comments go on new line. Set to true by default. |
| `bool NewLineBeforeInlineComment` | Gets or sets if inline comments go on a new line. Set to true by default. |
| `bool NewLineAfterInlineComment` | Gets or sets if a new line should follow inline comments. Set to true by default |
