using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using SqlJoiner.Presenter;
using System.ComponentModel;

namespace SqlJoiner.UI.Winforms
{
    public partial class Form1 : Form, IJoinerMainView
    {
        public Form1(ISchemaService schemaService, ITableService tableService)
        {
            InitializeComponent();
            new SqlJoinerMainPresenter(this, schemaService, tableService);

            this.button2.Click += GenerateSql;
        }

        private List<SchemaOL>? _schemaList;
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

                lbSchema.DataSource = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOL>? TableList { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaOL? SelectedSchema
        {
            get
            {
                return (SchemaOL?)cmbSchemaList.SelectedItem;
            }
            set
            {
                cmbSchemaList.SelectedItem = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TableOL? SelectedTable { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SqlJoinedResult { get; set; }

        public event EventHandler? Init;
        public event EventHandler? GenerateSql;
        public event EventHandler? LoadSchema;
        public event EventHandler? LoadTable;
        public event EventHandler? LoadColumns;
        public event EventHandler? SelectedSchemaValueChanged;

        private void Form1_Load(object sender, EventArgs e)
        {
            Init?.Invoke(sender, e);
            cmbSchemaList.DataSource = SchemaList;
            cmbSchemaList.DisplayMember = "SchemaName";
        }
    }
}
