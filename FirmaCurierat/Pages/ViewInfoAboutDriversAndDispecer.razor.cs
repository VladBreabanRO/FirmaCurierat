using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public partial class ViewInfoAboutDriversAndDispecerComponent:ComponentBase
    {
        public class infoForTheGrid
        {
            public string nume
            {
                get;
                set;
            }
            public string prenume
            {
                get;
                set;

            }
            public string nume_dispecer
            {
                get;
                set;

            }
        }
        public class forGrid2
        {
            public string nume_dispecer
            {
                get;
                set;
            }
            public string id_comanda
            {
                get;
                set;
            }
        }
        public class forGrid3
        {
            public string nume_dispecer
            {
                get;
                set;
            }
        }

        public class forGrid4
        {
            public string nume
            {
                get;
                set;

            }

            public string prenume
            {
                get;
                set;
            }
            public string awb
            {
                get;
                set;
            }
        }
        public class forGrid5
        {
            public string nume
            {
                get;
                set;

            }
            public string prenume
            {
                get;
                set;
            }
            public string marca
            {
                get;
                set;
            }
            public string an
            {
                get;
                set;
            }
        }
        public List<forGrid5> list5;
        public List<forGrid4> list4;
        public List<forGrid3> list3;
        public List<forGrid2> list2;
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        public List<infoForTheGrid> interestList;
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            dataHelper = new DataBaseManagement.DataManagement();
            interestList = new List<infoForTheGrid>();
            list2 = new List<forGrid2>();
            list3 = new List<forGrid3>();
            string ServerName = Environment.MachineName;
            
            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
         
            
            string sqlCommand = "select a.nume_dispecer, b.nume, b.prenume from dispeceri a " +
                "inner join soferi b on a.id_dispecer = b.id_dispecer";
            interestList = await dataHelper.LoadData<infoForTheGrid, dynamic>(sqlCommand, new { }, ConnectionString);

            sqlCommand = "select a.nume_dispecer, b.id_comanda from dispeceri a inner join comenzi b on a.id_dispecer = b.id_dispecer";
            list2 = await dataHelper.LoadData<forGrid2, dynamic>(sqlCommand, new { }, ConnectionString);

            sqlCommand = "select a.nume_dispecer,count(id_comanda) as id_co " +
                "from dispeceri a inner join comenzi c on a.id_dispecer = c.id_dispecer group by a.nume_dispecer having count(id_comanda) >=5";
                list3= await dataHelper.LoadData<forGrid3, dynamic>(sqlCommand, new { }, ConnectionString);

            list4 = new List<forGrid4>();

            sqlCommand = "select a.nume, a.prenume, b.awb from clienti a inner join comenzi b on a.id_client = b.id_client";
            list4 = await dataHelper.LoadData<forGrid4, dynamic>(sqlCommand, new { }, ConnectionString);

            list5 = new List<forGrid5>();
            sqlCommand = "select a.marca, a.an, b.nume, b.prenume from masini a left join soferi b on a.id_masina = b.id_masina group by a.marca, a.an, b.nume, b.prenume having a.an <= 2020";
            list5 = await dataHelper.LoadData<forGrid5, dynamic>(sqlCommand, new { }, ConnectionString);
        }

    }
}
