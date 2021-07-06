using Microsoft.AspNetCore.Components;
using Radzen;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class ViewNumberofOrdersComponent : ComponentBase
    {
        public List<Models.FirmaCurierat.Clienti> clients;
        public List<Models.FirmaCurierat.Cities> cities;
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        public Models.FirmaCurierat.Cities tara;
        public bool isGridVisible = false;
        public List<FirmaCurierat.Models.FirmaCurierat.OrdersByCity> listForGrid;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            tara = new Models.FirmaCurierat.Cities();
            dataHelper = new DataBaseManagement.DataManagement();
            cities = new List<Models.FirmaCurierat.Cities>();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

            clients = new List<FirmaCurierat.Models.FirmaCurierat.Clienti>();
            string sqlCommand = "select * from  cities";
           cities = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.Cities, dynamic>(sqlCommand, new { }, ConnectionString);
        }
       public Syncfusion.Blazor.Grids.SfGrid<FirmaCurierat.Models.FirmaCurierat.OrdersByCity> grid;
        public async Task visibleGrid(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, Models.FirmaCurierat.Cities> args)
        {
            if(isGridVisible == true)
            {
                listForGrid = new List<Models.FirmaCurierat.OrdersByCity>();
                string ServerName = Environment.MachineName;

                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);


                string sqlCommand = "select a.nume, a.prenume, b.id_comanda from clienti a " +
                    "inner join comenzi b on a.id_client = b.id_client" +
                    " where a.oras= " + "'" + tara.nume + "'";
                listForGrid = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.OrdersByCity, dynamic>(sqlCommand, new { }, ConnectionString);
                grid.Refresh();
                this.StateHasChanged();
            }
            else
            {
                listForGrid = new List<Models.FirmaCurierat.OrdersByCity>();
                string ServerName = Environment.MachineName;

                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);


                string sqlCommand = "select a.nume, a.prenume, b.id_comanda from clienti a " +
                    "inner join comenzi b on a.id_client = b.id_client" +
                    " where a.oras= " +"'"+tara.nume+"'";
                listForGrid = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.OrdersByCity, dynamic>(sqlCommand, new { }, ConnectionString);
                isGridVisible = true;
            }
        }
        }
}
