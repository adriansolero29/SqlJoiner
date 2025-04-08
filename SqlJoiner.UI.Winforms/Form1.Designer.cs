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
            button2 = new Button();
            treeData = new TreeView();
            txtSql = new RichTextBox();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(12, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "reload";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // treeData
            // 
            treeData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeData.CheckBoxes = true;
            treeData.FullRowSelect = true;
            treeData.HideSelection = false;
            treeData.Location = new Point(12, 41);
            treeData.Name = "treeData";
            treeData.ShowNodeToolTips = true;
            treeData.Size = new Size(249, 480);
            treeData.TabIndex = 6;
            treeData.BeforeCheck += treeData_BeforeCheck;
            treeData.AfterCheck += treeData_AfterCheck;
            treeData.BeforeCollapse += treeData_BeforeCollapse;
            treeData.AfterCollapse += treeData_AfterCollapse;
            treeData.BeforeExpand += treeData_BeforeExpand;
            treeData.AfterExpand += treeData_AfterExpand;
            treeData.NodeMouseClick += treeData_NodeMouseClick;
            // 
            // txtSql
            // 
            txtSql.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSql.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSql.Location = new Point(267, 41);
            txtSql.Name = "txtSql";
            txtSql.Size = new Size(280, 480);
            txtSql.TabIndex = 7;
            txtSql.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(559, 533);
            Controls.Add(txtSql);
            Controls.Add(treeData);
            Controls.Add(button2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private TreeView treeData;
        private RichTextBox txtSql;
    }
}
