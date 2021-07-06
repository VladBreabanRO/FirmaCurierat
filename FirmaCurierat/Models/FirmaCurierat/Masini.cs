using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models.FirmaCurierat
{
    [Table("masini", Schema = "dbo")]
    public class Masini
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_masina
        {
            get;
            set;
        }
        public int? an
        {
            get;
            set;
        }
        public string? marca
        {
            get;
            set;
        }
    }
}
