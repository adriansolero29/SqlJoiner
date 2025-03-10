using SqlJoiner.Models.Base;
using System.Security.AccessControl;

namespace SqlJoiner.Models
{
    public class CustomDatabaseEntityModelOL : IModel
    {
        private SchemaOL? _schemaInfo;
        public SchemaOL? SchemaInfo
        {
            get { return _schemaInfo; }
            set { _schemaInfo = value; }
        }

        private List<CustomDataTableEntityOL>? _recurseTableInformation;
        public List<CustomDataTableEntityOL>? RecurseTableInformation
        {
            get { return _recurseTableInformation; }
            set { _recurseTableInformation = value; }
        }

    }

    public class CustomDataTableEntityOL : IModel
    {
        private TableOL? _tableInformation;
        public TableOL? TableInformation
        {
            get { return _tableInformation; }
            set { _tableInformation = value; }
        }

        private List<ColumnOL>? _columnList;
        public List<ColumnOL>? ColumnList
        {
            get { return _columnList; }
            set { _columnList = value; }
        }

        private List<TableOL>? _foreignKeyTables;
        public List<TableOL>? ForeignKeyTables
        {
            get { return _foreignKeyTables; }
            set { _foreignKeyTables = value; }
        }

    }
}
