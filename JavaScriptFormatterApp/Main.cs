// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using SoftCircuits.JavaScriptFormatter;

namespace JavaScriptFormatterApp
{
    public partial class Main : Form
    {
        private readonly FormatOptions FormatOptions;

        public Main()
        {
            InitializeComponent();

            FormatOptions = new();
        }

        private void Options_Click(object sender, EventArgs e)
        {
            Options form = new(FormatOptions);
            form.ShowDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormatJavaScript_Click(object sender, EventArgs e)
        {
            try
            {
                JavaScriptFormatter formatter = new(FormatOptions);
                txtFormatted.Text = formatter.Format(txtUnformatted.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
