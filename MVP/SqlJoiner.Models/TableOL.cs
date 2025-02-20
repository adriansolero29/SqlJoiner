using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
    public class TableOL : IModel
    {
		private SchemaOL? _schema;
		public SchemaOL? Schema
		{
			get { return _schema; }
			set { _schema = value; }
		}

		private string? _name;
		public string? Name
		{
			get { return _name; }
			set { _name = value; }
		}
	}
}
