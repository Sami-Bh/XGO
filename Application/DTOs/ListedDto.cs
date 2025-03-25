using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ListedDto<T> where T :BaseDto
    {
        public int PageCount { get; set; }
        public IList<T> Items { get; set; } = [];
    }
}
