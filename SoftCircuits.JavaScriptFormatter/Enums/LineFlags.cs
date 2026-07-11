// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace SoftCircuits.JavaScriptFormatter.Enum
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// State flags for the current line.
    /// </summary>
    [Flags]
    internal enum LineFlags
    {
        None = 0,
        BlockKeyword = 1,
        DoKeyword = 2,
        EndDoBlock = 4,
        CaseKeyword = 8,
        JsonColon = 16,
        QuestionMark = 32,
    }
}
