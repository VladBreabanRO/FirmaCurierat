using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public partial class RequestedComponent : ComponentBase
    {
        public class infoForTheGrid
        {
            public string nume
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
        }

        public class infoForTheGrid2
        {
            public string marca
            {
                get;
                set;
            }
            public string valoare_comanda
            {
                get;
                set;
            }
         
        }
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        public List<infoForTheGrid> interestList;
        public List<infoForTheGrid2> interestList2;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            interestList = new List<infoForTheGrid>();
            interestList2 = new List<infoForTheGrid2>();
            dataHelper = new DataBaseManagement.DataManagement();
            string ServerName = Environment.MachineName;
            //interogari complexe
            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);


                               string sqlCommand = "select a.nume, b.awb, b.valoare_comanda from clienti a " +
                                             "inner join comenzi b on a.id_client = b.id_client where b.valoare_comanda >= ( select avg(valoare_comanda) from comenzi) order by " +
                                                "b.valoare_comanda asc";
            interestList = await dataHelper.LoadData<infoForTheGrid, dynamic>(sqlCommand, new { }, ConnectionString);
            sqlCommand = "select a.marca,b.valoare_comanda from masini a inner join soferi c on a.id_masina = c.id_masina inner join comenzi b on b.valoare_comanda >= (select max(valoare_comanda) from comenzi) ";
            interestList2 = await dataHelper.LoadData<infoForTheGrid2, dynamic>(sqlCommand, new { }, ConnectionString);
        }
        }
}
