// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

namespace SoftCircuits.JavaScriptFormatter
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
