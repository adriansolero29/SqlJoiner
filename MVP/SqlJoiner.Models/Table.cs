using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Models
{
    public class Table
    {
		private string? _name;
		public string? Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private IEnumerable<Column>? _columnList;
		public IEnumerable<Column>? ColumnList
		{
			get { return _columnList; }
			set { _columnList = value; }
		}

	}
}
