namespace SqlJoiner.UI.Winforms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbSchemaList = new ComboBox();
            lbSchema = new ListBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // cmbSchemaList
            // 
            cmbSchemaList.FormattingEnabled = true;
            cmbSchemaList.Location = new Point(12, 12);
            cmbSchemaList.Name = "cmbSchemaList";
            cmbSchemaList.Size = new Size(204, 23);
            cmbSchemaList.TabIndex = 0;
            // 
            // lbSchema
            // 
            lbSchema.FormattingEnabled = true;
            lbSchema.Location = new Point(12, 41);
            lbSchema.Name = "lbSchema";
            lbSchema.Size = new Size(204, 109);
            lbSchema.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(12, 156);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(228, 267);
            Controls.Add(button2);
            Controls.Add(lbSchema);
            Controls.Add(cmbSchemaList);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private ComboBox cmbSchemaList;
        private ListBox lbSchema;
        private Button button2;
    }
}
