using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Pagination
{
	public class Builder
	{

		private BasePageItemType _pageItemType;

		private int _index;

		private bool _active;

		public Builder pageItemType(BasePageItemType pageItemType)
		{
			this._pageItemType = pageItemType;
			return this;
		}

		public Builder index(int index)
		{
			this._index = index;
			return this;
		}

		public Builder active(bool active)
		{
			this._active = active;
			return this;
		}

		public BasePageItem build()
		{
			return new BasePageItem(_pageItemType, _index, _active);
		}
	}

	public class BasePageItem
	{

		public BasePageItemType pageItemType { get; set; }

		public int index { get; set; }

		public bool active { get; set; }

		public BasePageItem()
		{
			//
		}

		public BasePageItem(BasePageItemType pageItemType, int index, bool active)
		{
			this.pageItemType = pageItemType;
			this.index = index;
			this.active = active;
		}

		public static Builder builder()
		{
			return new Builder();
		}
	}
}
