using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using SqlJoiner.Presenter;
using System.ComponentModel;
using System.Diagnostics;

namespace SqlJoiner.UI.Winforms
{
    public partial class Form1 : Form, IJoinerMainView
    {
        public Form1(ISchemaService schemaService, ITableService tableService, IColumnService columnService)
        {
            InitializeComponent();
            new SqlJoinerMainPresenter(this, schemaService, tableService, columnService);
            this.tableService = tableService;
        }

        private List<SchemaOL>? _schemaList;
        private readonly ITableService tableService;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SchemaOL>? SchemaList
        {
            get
            {
                return _schemaList ?? new List<SchemaOL>();
            }
            set
            {
                _schemaList = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? TableList { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaOL? SelectedSchema
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TableOL? SelectedTable { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SqlJoinedResult { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CustomDatabaseEntityModelOL>? FullDatabaseEntityModel { get; set; }

        public event EventHandler? Init;
        public event EventHandler? GenerateSql;
        public event EventHandler? LoadSchema;
        public event EventHandler? LoadTable;
        public event EventHandler? LoadColumns;
        public event EventHandler? SelectedSchemaValueChanged;

        private async void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void TestProgressBar(IProgress<int> progress, object sender, EventArgs e)
        {
            //for (int i = 0; i < 100; i++)
            //{
            Init?.Invoke(sender, e);
            await Task.Delay(1000);

            if (FullDatabaseEntityModel == null) return;
            foreach (var item in FullDatabaseEntityModel)
            {
                var tables = item.RecurseTableInformation;
                var schemaNode = treeData.Nodes.Add(item?.SchemaInfo?.SchemaName);

                if (tables == null) return;
                foreach (var table in tables)
                {
                    var columns = table.ColumnList;
                    var tableNode = schemaNode.Nodes.Add(table.TableInformation?.Name);

                    if (columns == null) return;
                    foreach (var column in columns)
                    {
                        tableNode.Nodes.Add(column.ColumnName);
                    }
                }
            }
            //Thread.Sleep(100);
            //if (progress != null)
            //    progress.Report(i);
            //}
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            //loading.Value = 0;
            //var prog = new Progress<int>(percent => loading.Value = percent);

            //await Task.Run(() => TestProgressBar(prog, sender, e));
            Init?.Invoke(sender, e);
            await Task.Delay(1000);

            if (FullDatabaseEntityModel == null) return;
            foreach (var item in FullDatabaseEntityModel)
            {
                var tables = item.RecurseTableInformation;
                Debug.WriteLine("Schema: " + item.SchemaInfo?.SchemaName);
                var schemaNode = treeData.Nodes.Add(item?.SchemaInfo?.SchemaName);

                if (tables == null) return;
                foreach (var table in tables)
                {
                    var columns = table.ColumnList;
                    Debug.WriteLine("Table: " + table.TableInformation?.Name);
                    var tableNode = schemaNode.Nodes.Add(table.TableInformation?.Name);

                    if (columns == null) return;
                    foreach (var column in columns)
                    {
                        Debug.WriteLine("Column: " + column.ColumnName);
                        tableNode.Nodes.Add(column.ColumnName);
                    }
                }
            }
        }
    }
}
