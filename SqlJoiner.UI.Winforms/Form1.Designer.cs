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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnReset = new Button();
            btnGenerate = new Button();
            treeData = new TreeView();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            txtPanel = new Panel();
            panel1 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            btnExecute = new Button();
            outputDatagrid = new DataGridView();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)outputDatagrid).BeginInit();
            SuspendLayout();
            // 
            // btnReset
            // 
            btnReset.Location = new Point(355, 7);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(68, 36);
            btnReset.TabIndex = 9;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(23, 7);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(326, 36);
            btnGenerate.TabIndex = 8;
            btnGenerate.Text = "Generate SQL Query";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // treeData
            // 
            treeData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeData.BorderStyle = BorderStyle.None;
            treeData.CheckBoxes = true;
            treeData.Cursor = Cursors.Hand;
            treeData.FullRowSelect = true;
            treeData.HideSelection = false;
            treeData.Location = new Point(0, 0);
            treeData.Name = "treeData";
            treeData.ShowNodeToolTips = true;
            treeData.Size = new Size(446, 451);
            treeData.TabIndex = 6;
            treeData.AfterCheck += treeData_AfterCheck;
            treeData.AfterCollapse += treeData_AfterCollapse;
            treeData.AfterExpand += treeData_AfterExpand;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(971, 25);
            toolStrip1.TabIndex = 10;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(104, 22);
            toolStripButton1.Text = "Database Settings";
            // 
            // txtPanel
            // 
            txtPanel.Dock = DockStyle.Top;
            txtPanel.Location = new Point(0, 0);
            txtPanel.Name = "txtPanel";
            txtPanel.Size = new Size(525, 249);
            txtPanel.TabIndex = 11;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(treeData);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 25);
            panel1.Name = "panel1";
            panel1.Size = new Size(446, 508);
            panel1.TabIndex = 12;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnGenerate);
            panel3.Controls.Add(btnReset);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 457);
            panel3.Name = "panel3";
            panel3.Size = new Size(446, 51);
            panel3.TabIndex = 10;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(txtPanel);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(446, 25);
            panel2.Name = "panel2";
            panel2.Size = new Size(525, 508);
            panel2.TabIndex = 13;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnExecute);
            panel4.Controls.Add(outputDatagrid);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 249);
            panel4.Name = "panel4";
            panel4.Size = new Size(525, 259);
            panel4.TabIndex = 12;
            // 
            // btnExecute
            // 
            btnExecute.Location = new Point(6, 6);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(95, 27);
            btnExecute.TabIndex = 10;
            btnExecute.Text = "Run Query";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += btnExecute_Click;
            // 
            // outputDatagrid
            // 
            outputDatagrid.AllowUserToAddRows = false;
            outputDatagrid.AllowUserToDeleteRows = false;
            outputDatagrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            outputDatagrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            outputDatagrid.BorderStyle = BorderStyle.None;
            outputDatagrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            outputDatagrid.Location = new Point(0, 46);
            outputDatagrid.Margin = new Padding(10);
            outputDatagrid.Name = "outputDatagrid";
            outputDatagrid.Size = new Size(525, 213);
            outputDatagrid.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 533);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)outputDatagrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExecute;
        private Button btnReset;
        private Button btnGenerate;
        private TreeView treeData;
        private ToolStrip toolStrip1;
        private Panel txtPanel;
        private ToolStripButton toolStripButton1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private DataGridView outputDatagrid;
    }
}
