using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Models
{
    public class Column
    {
		private string? _name;
		public string? Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string? _dataType;
		public string? DataType
		{
			get { return _dataType; }
			set { _dataType = value; }
		}

	}
}
