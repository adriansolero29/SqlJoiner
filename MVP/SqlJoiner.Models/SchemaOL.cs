using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
    public class SchemaOL : IModel
    {
		private string? _catalogName;
		public string? CatalogName
		{
			get { return _catalogName; }
			set { _catalogName = value; }
		}

		private string? _schemaName;
		public string? SchemaName
		{
			get { return _schemaName; }
			set { _schemaName = value; }
		}

		private string? _schemaOwner;
		public string? SchemaOwner
		{
			get { return _schemaOwner; }
			set { _schemaOwner = value; }
		}
	}
}
