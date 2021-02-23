// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.JavaScriptFormatter;
using System;
using System.Windows.Forms;

namespace TestJavaScriptFormatter
{
    public partial class Options : Form
    {
        FormatOptions FormatOptions;

        public Options(FormatOptions formatOptions)
        {
            InitializeComponent();

            FormatOptions = formatOptions;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            txtTab.Text = FormatOptions.Tab.Replace("\t", "\\t");
            chkNewLineBetweenFunctions.Checked = FormatOptions.NewLineBetweenFunctions;
            chkOpenBraceOnNewLine.Checked = FormatOptions.OpenBraceOnNewLine;
            chkNewLineBeforeLineComment.Checked = FormatOptions.NewLineBeforeLineComment;
            chkNewLineBeforeInlineComment.Checked = FormatOptions.NewLineBeforeInlineComment;
            chkNewLineAfterInlineComment.Checked = FormatOptions.NewLineAfterInlineComment;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FormatOptions.Tab = txtTab.Text.Replace("\\t", "\t");
            FormatOptions.NewLineBetweenFunctions = chkNewLineBetweenFunctions.Checked;
            FormatOptions.OpenBraceOnNewLine = chkOpenBraceOnNewLine.Checked;
            FormatOptions.NewLineBeforeLineComment = chkNewLineBeforeLineComment.Checked;
            FormatOptions.NewLineBeforeInlineComment = chkNewLineBeforeInlineComment.Checked;
            FormatOptions.NewLineAfterInlineComment = chkNewLineAfterInlineComment.Checked;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
