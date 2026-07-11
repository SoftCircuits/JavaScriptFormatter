// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace SoftCircuits.JavaScriptFormatter.Enum
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// State flags for the current indent block.
    /// </summary>
    [Flags]
    internal enum IndentFlags
    {
        None = 0,
        NoBraces = 1,
        DoBlock = 2,
        CaseBlock = 4,
        BracketBlock = 8,
    }
}
