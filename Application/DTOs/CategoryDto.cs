using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoryDto : BaseDto
    {        
        public required string Name
        {
            get; set;
        }
        public bool HasChildren { get; set; }

    }
}
