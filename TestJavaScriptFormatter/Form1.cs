// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.JavaScriptFormatter;
using System;
using System.Windows.Forms;

namespace TestJavaScriptFormatter
{
    public partial class Form1 : Form
    {
        private readonly FormatOptions FormatOptions;

        public Form1()
        {
            InitializeComponent();

            FormatOptions = new FormatOptions();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options form = new Options(FormatOptions);
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void formatJavaScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                JavaScriptFormatter formatter = new JavaScriptFormatter(FormatOptions);
                txtFormatted.Text = formatter.Format(txtUnformatted.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
