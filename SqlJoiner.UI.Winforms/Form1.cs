using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using SqlJoiner.Presenter;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SchemaOL>? SchemaList { get; set; } = new List<SchemaOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? TableList { get; set; } = new List<TableOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ColumnList { get; set; } = new List<ColumnOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ColumnOL>? ForeignKeyColumnsList { get; set; } = new List<ColumnOL>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaOL? CurrentSelectedSchema { get; set; } = new SchemaOL();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TableOL? CurrentSelectedTable { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomRecurseColumnOL CurrentSelectedColumn { get; set; } = new CustomRecurseColumnOL();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnOL? SelectedColumn2 { get; set; } = new ColumnOL();

        public event EventHandler? Init;
        public event EventHandler? SelectSchema;
        public event EventHandler? SelectTable;
        public event EventHandler? SelectColumn;
        public event EventHandler? GenerateQuery;
        public event EventHandler? LoadTableBySelectedSchema;
        public event EventHandler? LoadColumnFromSelectedTable;
        public event EventHandler? LoadForeignKeyColumnsBySelectedForeignKey;

        private async void Form1_Load(object sender, EventArgs e)
        {
            Init?.Invoke(sender, e);

            treeData.Nodes.Clear();

            var treeNodes = await Task.Run(() =>
            {
                var nodes = new List<TreeNode>();

                if (SchemaList == null) throw new Exception("No data provided");
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

            treeData.Invoke(() =>
            {
                treeData.Nodes.AddRange(treeNodes.ToArray());
            });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateQuery?.Invoke(sender, e);
        }

        private async void treeData_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                var selectedNodeType = selectedNode?.Tag?.GetType();
                if (selectedNodeType == typeof(SchemaOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;

                    CurrentSelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() =>
                    {
                        LoadTableBySelectedSchema?.Invoke(sender, e);
                    });

                    if (TableList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in TableList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.Name;

                        selectedNode?.Nodes.Add(tableNode);
                        tableNode.Nodes.Add(new TreeNode());
                    }
                }

                if (selectedNodeType == typeof(TableOL))
                {
                    if (selectedNode != null) selectedNode.Checked = true;
                    CurrentSelectedTable = selectedNode?.Tag as TableOL;

                    await Task.Run(() =>
                    {
                        LoadColumnFromSelectedTable?.Invoke(sender, e);
                    });

                    if (ColumnList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in ColumnList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(tableNode);
                        if (item.IsForeignKey)
                            tableNode.Nodes.Add(new TreeNode());
                    }
                }

                if (selectedNodeType == typeof(ColumnOL))
                {
                    CurrentSelectedColumn.ColumnInfo = selectedNode?.Tag as ColumnOL;

                    await Task.Run(() =>
                    {
                        LoadForeignKeyColumnsBySelectedForeignKey?.Invoke(sender, e);
                    });

                    if (ForeignKeyColumnsList == null) return;

                    selectedNode?.Nodes?.Clear();
                    foreach (var item in ForeignKeyColumnsList)
                    {
                        var tableNode = new TreeNode();
                        tableNode.Tag = item;
                        tableNode.Text = item.ColumnName;

                        selectedNode?.Nodes.Add(tableNode);
                        if (item.IsForeignKey)
                            tableNode.Nodes.Add(new TreeNode());
                    }
                }
            }
        }

        private async void treeData_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                var selectedNodeType = selectedNode?.Tag?.GetType();
                if (selectedNodeType == typeof(SchemaOL))
                {
                    CurrentSelectedSchema = selectedNode?.Tag as SchemaOL;
                    await Task.Run(() =>
                    {
                        SelectSchema?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(TableOL))
                {
                    CurrentSelectedTable = selectedNode?.Tag as TableOL;
                    await Task.Run(() =>
                    {
                        SelectTable?.Invoke(sender, e);
                    });
                }

                if (selectedNodeType == typeof(ColumnOL))
                {
                    CurrentSelectedColumn.ColumnInfo = selectedNode?.Tag as ColumnOL;
                    await Task.Run(() =>
                    {
                        SelectColumn?.Invoke(sender, e);
                    });
                }
            }
        }
    }
}
