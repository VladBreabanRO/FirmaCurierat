using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models.FirmaCurierat
{
    [Table("soferi", Schema = "dbo")]
    public class Soferi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id_sofer
        {
            get;
            set;
        }

        public string? nume
        {
            get;
            set;

        }
        public string? prenume
        {
            get;
            set;

        }
        public int id_masina
        {
            get;
            set;
        }
        public int id_dispecer
        {
            get;
            set;

        }
        public int? an_angajare
        {
            get;
            set;
        }
    }
}
