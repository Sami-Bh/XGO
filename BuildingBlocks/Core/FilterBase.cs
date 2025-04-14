using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Core
{
    public abstract class FilterBase : IFilterBase
    {
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;

        public virtual int GetSkip()
        {
            var pageSize = PageSize <= 0 || PageSize > 5 ? 5 : PageSize;
            var pageIndex = PageIndex > 0 ? PageIndex - 1 : 1;
            return pageSize * pageIndex;
        }

        public virtual int GetPageCount(int count)
        {

            return count > PageSize ? (int)Math.Ceiling((decimal)count / PageSize) : 1;
        }
    }
}
