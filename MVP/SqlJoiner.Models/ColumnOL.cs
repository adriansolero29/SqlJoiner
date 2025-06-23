using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
	[Serializable]
    public class ColumnOL : IModel
    {
		private TableOL? _table;
		public TableOL? Table
		{
			get { return _table; }
			set { _table = value; }
		}

		private string? _columnName;
		public string? ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		private int _ordinalPosition;
		public int OrdinalPosition
		{
			get { return _ordinalPosition; }
			set { _ordinalPosition = value; }
		}

		private string? _dataType;
		public string? DataType
		{
			get { return _dataType; }
			set { _dataType = value; }
		}

		private bool _isForeignKey;
		public bool IsForeignKey
		{
			get { return _isForeignKey; }
			set { _isForeignKey = value; }
		}

	}
}
