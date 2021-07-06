using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models.FirmaCurierat
{
    [Table("dispeceri", Schema = "dbo")]
    public class Dispeceri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_dispecer
        {
            get;
            set;
        }
        public string? nume_dispecer
        {
            get;
            set;
        }
    }
}
