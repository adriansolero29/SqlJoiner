using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using SqlJoiner.Presenter;
using System.ComponentModel;
using System.Diagnostics;

namespace SqlJoiner.UI.Winforms
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public partial class Form1 : Form, IJoinerMainView
    {
        public SqlJoinerMainPresenter Presenter { get; }

        public Form1(ISchemaService schemaService, ITableService tableService, IColumnService columnService)
        {
            InitializeComponent();
            Presenter = new SqlJoinerMainPresenter(this, schemaService, tableService, columnService);
        }

        private string? _sqlJoinedResult;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SqlJoinedResult
        {
            get
            {
                return _sqlJoinedResult;
            }
            set
            {
                _sqlJoinedResult = value;
                txtSql.Invoke(() =>
                {
                    txtSql.Text = value;
                });
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SchemaOL>? SchemaList { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? TableList { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaOL? SelectedSchema { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TableOL? SelectedTable { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CustomDatabaseEntityModelOL>? FullDatabaseEntityModel { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ColumnList { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? SelectedForeignKeyColumn { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnOL? SelectedColumn { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ColumnFromForeignKeyColumn { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? SelectedColumns { get; set; } = new List<ColumnOL>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? SelectedTables { get; set; } = new List<TableOL>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? SelectedForeignKeyColumns { get; set; }

        public event EventHandler? Init;
        public event EventHandler? GenerateSql;
        public event EventHandler? LoadSchema;
        public event EventHandler? LoadTable;
        public event EventHandler? LoadColumns;
        public event EventHandler? SelectedSchemaValueChanged;
        public event EventHandler? SelectedTableChanged;
        public event EventHandler? SelectedColumnChanged;
        public event EventHandler<ColumnOL>? ColumnSelection;
        public event EventHandler<ColumnOL>? ColumnRemoveSelection;
        public event EventHandler<TableOL>? TableSelection;
        public event EventHandler<TableOL>? TableRemoveSelection;

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Run(() => Init?.Invoke(sender, e));
            treeData.Nodes.Clear();

            button2.Enabled = false;

            var treeNodes = await Task.Run(() =>
            {
                var nodes = new List<TreeNode>();

                if (SchemaList == null) return new List<TreeNode>();

                foreach (var item in SchemaList)
                {
                    var schemaNode = new TreeNode();
                    schemaNode.Text = item.SchemaName;
                    schemaNode.Tag = item;

                    nodes.Add(schemaNode);
                    schemaNode.Nodes.Add(new TreeNode());
                }
                return nodes;
            });

            // Update UI in UI thread
            treeData.Invoke(() =>
            {
                treeData.Nodes.AddRange(treeNodes.ToArray());
            });

            button2.Enabled = true;
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {

        }

        private async void treeData_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //var selectedNode = e.Node;
            //var selectedNodeType = selectedNode?.Tag?.GetType();

            //if (selectedNode != null)
            //{
            
            //}
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                selectedNode.Checked = true;
                var selectedNodeType = selectedNode?.Tag?.GetType();

                if (selectedNodeType == typeof(SchemaOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;

                    SelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() => SelectedSchemaValueChanged?.Invoke(sender, e));

                    if (TableList == null) return;

                    selectedNode?.Nodes.Clear();
                    foreach (var item in TableList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.Name;

                        selectedNode?.Nodes.Add(tableNode);
                        tableNode.Nodes.Add(new TreeNode()); // add temporary child node
                    }
                }
                else if (selectedNodeType == typeof(TableOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;

                    SelectedTable = selectedNode?.Tag as TableOL;
                    await Task.Run(() => SelectedTableChanged?.Invoke(sender, e));

                    if (ColumnList == null) return;

                    selectedNode?.Nodes.Clear();
                    foreach (var item in ColumnList)
                    {
                        var columnNode = new TreeNode();
                        columnNode.Tag = item;
                        columnNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(columnNode);

                        var isForeignKey = await Presenter.checkIfColumnIsForeignKeyTable(item);
                        if (isForeignKey)
                        {
                            columnNode.Nodes.Add(new TreeNode()); // add temporary child node for foreign key columns
                        }
                    }

                }
                else if (selectedNodeType == typeof(ColumnOL))
                {
                    SelectedColumn = selectedNode?.Tag as ColumnOL;
                    await Task.Run(() => SelectedColumnChanged?.Invoke(sender, e));

                    if (ColumnFromForeignKeyColumn == null) return;

                    selectedNode?.Nodes.Clear();
                    foreach (var item in ColumnFromForeignKeyColumn)
                    {
                        var foreignKeyColumnColumnsNode = new TreeNode();
                        foreignKeyColumnColumnsNode.Tag = item;
                        foreignKeyColumnColumnsNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(foreignKeyColumnColumnsNode);
                        foreignKeyColumnColumnsNode.Nodes.Add(new TreeNode()); // add temporary child node
                    }
                }
                //    //if (selectedNodeType == typeof(ColumnOL) && selectedNode.Checked)
            //    //{
            //    selectedNode.Checked = false;
            //    SelectedColumns?.Add(selectedNode.Tag as ColumnOL ?? new ColumnOL());
            //    await Task.Run(() => ColumnSelection?.Invoke(sender, selectedNode.Tag as ColumnOL ?? new ColumnOL()));
            //    //}
            }
        }

        private async void treeData_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            
        }

        private void treeData_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            //var selectedNode = e.Node;
            //var selectedNodeType = selectedNode?.Tag?.GetType();
            //if (selectedNodeType == typeof(TableOL))
            //{
            //    TableSelection?.Invoke(sender, selectedNode?.Tag as TableOL ?? new TableOL());
            //}
        }

        private async void treeData_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            await Task.Delay(100);

            var selectedNode = e.Node;
            if (selectedNode != null)
                selectedNode.Checked = false;
        }

        private void treeData_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            var selectedNodeType = selectedNode?.Tag?.GetType();
            if (selectedNodeType == typeof(ColumnOL))
            {
                ColumnSelection?.Invoke(sender, selectedNode?.Tag as ColumnOL ?? new ColumnOL());
            }
        }

        private async void treeData_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            await Task.Delay(100);

            var selectedNode = e.Node;
            if (selectedNode != null)
                selectedNode.Checked = false;
        }

        private async void treeData_AfterExpand(object sender, TreeViewEventArgs e)
        {
           
        }
    }
}
