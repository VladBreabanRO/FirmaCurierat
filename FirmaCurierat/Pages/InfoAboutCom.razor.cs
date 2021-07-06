using AntDesign;
using AntDesign.core.JsInterop.EventArg;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class InfoAboutComComponent:ComponentBase
    {

        [Parameter]
        public dynamic awb { get; set; }
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected Radzen.NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        public class infoForTheGrid
        {
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
            public string data_livrare
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
            
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            //comanda sql folosita
            //select a.nume, a.prenume from soferi a where a.id_dispecer = (select c.id_dispecer from dispeceri c inner join comenzi d on c.id_dispecer = d.id_dispecer where d.awb = '123111' )
            string sqlCommand = "select a.nume_dispecer from dispeceri a where " +
                               " a.id_dispecer = (select c.id_dispecer from dispeceri c inner join comenzi d on c.id_dispecer = d.id_dispecer where d.awb= '" + awb + "' and d.id_client = (select id_client from clienti e where d.id_client = e.id_client ) ) ";
            //alocare 
            List<string> nume_dispecer = new List<string>();
            //obtinerea datelor din baza de date
            nume_dispecer = await dataHelper.LoadData<string, dynamic>(sqlCommand, new { }, ConnectionString);
            sqlCommand = "select a.valoare_comanda, a.awb,a.data_livrare from comenzi a where a.awb = '" + awb + "'";
            interestList = await dataHelper.LoadData<infoForTheGrid, dynamic>(sqlCommand, new { }, ConnectionString);
            if(interestList.Count == 0)
            {
                // notificare
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"We do not have an order with this awb");
                //return la pagina
                UriHelper.NavigateTo("/user");
            }
            else
            {
                interestList.ElementAt(0).nume_dispecer = nume_dispecer.ElementAt(0);
            }
           
            
        }

        protected async Task logOut(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/");
        }
        }
}
