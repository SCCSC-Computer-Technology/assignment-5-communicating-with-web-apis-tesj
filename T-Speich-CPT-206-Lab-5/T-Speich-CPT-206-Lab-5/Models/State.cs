using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T_Speich_CPT_206_Lab_5.Models
{
    public class State
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int State_ID { get; set; }

        [StringLength(50)]
        public string State_Name { get; set; }
        [Range(0, int.MaxValue)]
        public int State_Population { get; set; }
        public string State_Flag_Description { get; set; }
        [StringLength(50)]
        public string State_Flower { get; set; }
        [StringLength(50)]
        public string State_Bird { get; set; }
        [StringLength(50)]
        public string? State_Colors { get; set; }
        public string State_Largest_Cities { get; set; }
        [StringLength(50)]
        public string? State_Capital { get; set; }

        [Range(0, int.MaxValue)]
        public int State_Median_Income { get; set; }
        [Range(0, 100), Precision(2)]
        public double State_Computer_Jobs_Percent { get; set; }
    }
}
