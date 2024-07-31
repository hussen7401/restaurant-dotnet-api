
using System.ComponentModel.DataAnnotations;

namespace core.DTOs.TableDto
{
    public class TableDto
    {
        [Required]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int Capacity { get; set; }
    }
}
