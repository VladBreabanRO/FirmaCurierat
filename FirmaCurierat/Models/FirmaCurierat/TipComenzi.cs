using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models.FirmaCurierat
{
    [Table("tip_comenzi", Schema = "dbo")]
    public class TipComenzi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_tip
        {
            get;
            set;
        }
        public string tip
        {
            get;
            set;
        }
        public string specificatii
        {
            get;
            set;
        }
    }
}
