namespace JavaScriptFormatterApp

{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtTab = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkNewLineAfterInlineComment = new System.Windows.Forms.CheckBox();
            this.chkNewLineBeforeInlineComment = new System.Windows.Forms.CheckBox();
            this.chkNewLineBeforeLineComment = new System.Windows.Forms.CheckBox();
            this.chkOpenBraceOnNewLine = new System.Windows.Forms.CheckBox();
            this.chkNewLineBetweenFunctions = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Indent string (use \\t for tab):";
            // 
            // txtTab
            // 
            this.txtTab.Location = new System.Drawing.Point(172, 30);
            this.txtTab.Name = "txtTab";
            this.txtTab.Size = new System.Drawing.Size(73, 20);
            this.txtTab.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(91, 230);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(172, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkNewLineBetweenFunctions);
            this.groupBox1.Controls.Add(this.chkNewLineAfterInlineComment);
            this.groupBox1.Controls.Add(this.chkNewLineBeforeInlineComment);
            this.groupBox1.Controls.Add(this.chkNewLineBeforeLineComment);
            this.groupBox1.Controls.Add(this.chkOpenBraceOnNewLine);
            this.groupBox1.Location = new System.Drawing.Point(12, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Line breaks";
            // 
            // chkNewLineAfterInlineComment
            // 
            this.chkNewLineAfterInlineComment.AutoSize = true;
            this.chkNewLineAfterInlineComment.Location = new System.Drawing.Point(19, 120);
            this.chkNewLineAfterInlineComment.Name = "chkNewLineAfterInlineComment";
            this.chkNewLineAfterInlineComment.Size = new System.Drawing.Size(209, 17);
            this.chkNewLineAfterInlineComment.TabIndex = 4;
            this.chkNewLineAfterInlineComment.Text = "&New line should follow inline comments";
            this.chkNewLineAfterInlineComment.UseVisualStyleBackColor = true;
            // 
            // chkNewLineBeforeInlineComment
            // 
            this.chkNewLineBeforeInlineComment.AutoSize = true;
            this.chkNewLineBeforeInlineComment.Location = new System.Drawing.Point(18, 97);
            this.chkNewLineBeforeInlineComment.Name = "chkNewLineBeforeInlineComment";
            this.chkNewLineBeforeInlineComment.Size = new System.Drawing.Size(183, 17);
            this.chkNewLineBeforeInlineComment.TabIndex = 3;
            this.chkNewLineBeforeInlineComment.Text = "&Inline comments go on a new line";
            this.chkNewLineBeforeInlineComment.UseVisualStyleBackColor = true;
            // 
            // chkNewLineBeforeLineComment
            // 
            this.chkNewLineBeforeLineComment.AutoSize = true;
            this.chkNewLineBeforeLineComment.Location = new System.Drawing.Point(19, 74);
            this.chkNewLineBeforeLineComment.Name = "chkNewLineBeforeLineComment";
            this.chkNewLineBeforeLineComment.Size = new System.Drawing.Size(169, 17);
            this.chkNewLineBeforeLineComment.TabIndex = 2;
            this.chkNewLineBeforeLineComment.Text = "&Line comments go on new line";
            this.chkNewLineBeforeLineComment.UseVisualStyleBackColor = true;
            // 
            // chkOpenBraceOnNewLine
            // 
            this.chkOpenBraceOnNewLine.AutoSize = true;
            this.chkOpenBraceOnNewLine.Location = new System.Drawing.Point(19, 28);
            this.chkOpenBraceOnNewLine.Name = "chkOpenBraceOnNewLine";
            this.chkOpenBraceOnNewLine.Size = new System.Drawing.Size(182, 17);
            this.chkOpenBraceOnNewLine.TabIndex = 0;
            this.chkOpenBraceOnNewLine.Text = "&Opening braces go on a new line";
            this.chkOpenBraceOnNewLine.UseVisualStyleBackColor = true;
            // 
            // chkNewLineBetweenFunctions
            // 
            this.chkNewLineBetweenFunctions.AutoSize = true;
            this.chkNewLineBetweenFunctions.Location = new System.Drawing.Point(19, 51);
            this.chkNewLineBetweenFunctions.Name = "chkNewLineBetweenFunctions";
            this.chkNewLineBetweenFunctions.Size = new System.Drawing.Size(184, 17);
            this.chkNewLineBetweenFunctions.TabIndex = 1;
            this.chkNewLineBetweenFunctions.Text = "Insert new line between &functions";
            this.chkNewLineBetweenFunctions.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(338, 266);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTab);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTab;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkNewLineAfterInlineComment;
        private System.Windows.Forms.CheckBox chkNewLineBeforeInlineComment;
        private System.Windows.Forms.CheckBox chkNewLineBeforeLineComment;
        private System.Windows.Forms.CheckBox chkOpenBraceOnNewLine;
        private System.Windows.Forms.CheckBox chkNewLineBetweenFunctions;
    }
}