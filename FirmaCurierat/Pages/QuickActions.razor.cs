using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class QuickActionsComponent : ComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        //serie de functii pentru navigare in diferite pagini
        public List<Models.FirmaCurierat.Comenzi> comenzi;
        public async Task viewDrivers(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/employees");

        }
        public async Task viewOrders(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/view-number");

        }
        public async Task viewGeneral(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/view-general");

        }

        public async Task viewOrderValues(MouseEventArgs args)
        {
            DialogService.OpenAsync<Requested>("The most valuable orders");
        }
        public async Task registerCar(MouseEventArgs args)
        {
            DialogService.OpenAsync<AddCar>("Register new car");
        }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            dataHelper = new DataBaseManagement.DataManagement();
            comenzi = new List<Models.FirmaCurierat.Comenzi>();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            string sqlCommand = "select * from comenzi";
            comenzi = await dataHelper.LoadData<Models.FirmaCurierat.Comenzi, dynamic>(sqlCommand, new { }, ConnectionString);
        }
    }
}
