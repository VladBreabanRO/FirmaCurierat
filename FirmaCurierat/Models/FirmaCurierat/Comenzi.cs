using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Models.FirmaCurierat
{
    [Table("comenzi", Schema = "dbo")]

    public class Comenzi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_comanda
        {
            get;
            set;
        }
        public string data_livrare
        {
            get;
            set;
        }
        public int id_dispecer
        {
            get;
            set;
        }
        public int id_client
        {
            get;
            set;
        }
        public string awb
        {
            get;
            set;
        }
        public int valoare_comanda
        {
            get;
            set;
        }
        public int id_sofer
        {
            get;
            set;
        }
    }
}
