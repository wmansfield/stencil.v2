using System;

namespace CodeGenerator
{
	public class Template
	{
		public bool IsSelected
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Template() : this(string.Empty, string.Empty, false)
		{
		}

		public Template(string name, string location) : this(name, location, false)
		{
		}

		public Template(string name, string location, bool isSelected)
		{
			this.Name = name;
			this.Location = location;
			this.IsSelected = isSelected;
		}

		public override string ToString()
		{
			string result;
			if (!string.IsNullOrEmpty(this.Location))
			{
				result = this.Location;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
