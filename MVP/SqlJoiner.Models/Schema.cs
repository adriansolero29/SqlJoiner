using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Models
{
    public class Schema
    {
		private string? _name;
		public string? Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private IEnumerable<Table>? _tableList;
		public IEnumerable<Table>? TableList
		{
			get { return _tableList; }
			set { _tableList = value; }
		}
	}
}
