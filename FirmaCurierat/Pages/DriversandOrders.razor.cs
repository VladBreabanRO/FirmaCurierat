using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class DriversandOrdersComponent : ComponentBase
    {
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        public class interestList
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
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        public List<interestList> interest;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {  //initializare datahelper pt a putea accesa functia generica de get a unei liste din baaz de date
            dataHelper = new DataBaseManagement.DataManagement();
            // SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

            interest = new List<interestList>();
            string sqlCommand = "select a.nume, a.prenume, b.awb from soferi a inner join dispeceri d on d.id_dispecer = (select c.id_dispecer from dispeceri c where a.id_dispecer = c.id_dispecer ) inner join comenzi b on(d.id_dispecer = b.id_dispecer) where b.id_sofer is not null and a.id_sofer = b.id_sofer";
            interest = await dataHelper.LoadData<interestList, dynamic>(sqlCommand, new { }, ConnectionString);
        }
        }
}
