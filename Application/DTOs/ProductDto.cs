using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductDto:BaseDto
    {
        public required string Name
        {
            get; set;
        }
        public string? ExtraProperties
        {
            get; set;
        }
        public bool IsProximity { get; set; }
        public bool IsHeavy { get; set; }
        public bool IsBulky { get; set; }
        public int SubCategoryId { get; set; } // Required foreign key property
    }
}
