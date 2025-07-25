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
            treeData = new TreeView();
            txtSql = new RichTextBox();
            btnGenerate = new Button();
            SuspendLayout();
            // 
            // treeData
            // 
            treeData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeData.CheckBoxes = true;
            treeData.Cursor = Cursors.Hand;
            treeData.FullRowSelect = true;
            treeData.HideSelection = false;
            treeData.Location = new Point(12, 48);
            treeData.Name = "treeData";
            treeData.ShowNodeToolTips = true;
            treeData.Size = new Size(337, 473);
            treeData.TabIndex = 6;
            treeData.AfterCheck += treeData_AfterCheck;
            treeData.AfterExpand += treeData_AfterExpand;
            // 
            // txtSql
            // 
            txtSql.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSql.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSql.Location = new Point(355, 12);
            txtSql.Name = "txtSql";
            txtSql.Size = new Size(192, 509);
            txtSql.TabIndex = 7;
            txtSql.Text = "";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(12, 12);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(337, 30);
            btnGenerate.TabIndex = 8;
            btnGenerate.Text = "Generate SQL Query";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(559, 533);
            Controls.Add(btnGenerate);
            Controls.Add(txtSql);
            Controls.Add(treeData);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private TreeView treeData;
        private RichTextBox txtSql;
        private Button btnGenerate;
    }
}
