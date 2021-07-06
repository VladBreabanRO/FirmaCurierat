using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models
{
    [Table("tipul_comenzii", Schema = "dbo")]
    public class TipulComenzii
    {
        public int? id_tip
        {
            get;
            set;

        }
        public int? id_comanda
        {
            get;set;
        }
    }
}
