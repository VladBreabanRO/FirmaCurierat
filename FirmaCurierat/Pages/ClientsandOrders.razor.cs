using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public partial class ClientsandOrdersComponent : ComponentBase
    {
        public List<Models.FirmaCurierat.Clienti> clients;

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
        public Syncfusion.Blazor.Grids.SfGrid<Models.FirmaCurierat.Clienti> grid;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            dataHelper = new DataBaseManagement.DataManagement();
            // SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

            clients = new List<FirmaCurierat.Models.FirmaCurierat.Clienti>();
            string sqlCommand = "select * from  clienti";
            clients = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.Clienti, dynamic>(sqlCommand, new { }, ConnectionString);
        }
        public async Task goToAdd(MouseEventArgs args)
        {

            NavigationManager.NavigateTo("/addOrdersAndClients");
            await InvokeAsync(() => { StateHasChanged(); });
        }
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, object data)
        {

            try
            {
                if (await DialogService.Confirm("Do you want to delete this record?") == true)
                {
                    //functie delete client and order
                    FirmaCurierat.Models.FirmaCurierat.Clienti data2 = new Models.FirmaCurierat.Clienti();
                    // drivers2.RemoveAll(d => d.id_sofer == data2.id_sofer);
                    data2 = (Models.FirmaCurierat.Clienti)data;
                    SqlConnection scn = new SqlConnection();
                    string ServerName = Environment.MachineName;

                    string database = "CurieratVladProiect";
                    string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

                    scn.ConnectionString = ConnectionString;
                    //selectez comanda care urmeaza sa fie stearsa
                    SqlCommand scmd = new SqlCommand("select id_comanda from comenzi where id_client = @id", scn);
                    scmd.Parameters.Clear();
                    scmd.Parameters.AddWithValue("@id", data2.id_client);
                    scn.Open();
                    int deletType = Convert.ToInt32(scmd.ExecuteScalar());
                    //sterg din tipul comenzii conform relatiei
                    scmd = new SqlCommand("delete from tipul_comenzii where id_comanda = @id", scn);
                    scmd.Parameters.AddWithValue("@id", deletType);
                    scmd.ExecuteNonQuery();
                    //sterg din comenzi
                    scmd = new SqlCommand("delete from comenzi where id_comanda = @id", scn);
                    scmd.Parameters.AddWithValue("@id", deletType);
                    scmd.ExecuteNonQuery();
                    scmd = new SqlCommand("delete from clienti where id_client = @id", scn);
                    scmd.Parameters.AddWithValue("@id", data2.id_client);
                    scmd.ExecuteNonQuery();
                    this.StateHasChanged();

                    clients = new List<FirmaCurierat.Models.FirmaCurierat.Clienti>();
                    string sqlCommand = "select * from  clienti";
                    clients = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.Clienti, dynamic>(sqlCommand, new { }, ConnectionString);
                    grid.Refresh();
                    NotificationService.Notify(NotificationSeverity.Success, $"Order deleted");

                }
            }
            catch (System.Exception rlvMailerDeleteMailException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete order and client");
            }
        }
        //functie  pentru a accesa pagna de editare a clientului
        protected async System.Threading.Tasks.Task updateStuff(MouseEventArgs args, object data)
        {
            FirmaCurierat.Models.FirmaCurierat.Clienti data2 = new Models.FirmaCurierat.Clienti();
            // drivers2.RemoveAll(d => d.id_sofer == data2.id_sofer);
            data2 = (Models.FirmaCurierat.Clienti)data;
         UriHelper.NavigateTo($"edit-client/{data2.id_client}");
            //var dialogResult = await DialogService.OpenAsync<EditClientandOrder>("Edit Order", new Dictionary<string, object>() { { "client_id", data2.id_client} });
            await InvokeAsync(() => { StateHasChanged(); });
        }
        //functie pentru accesarea paginii de  editare a comenzii
        protected async System.Threading.Tasks.Task updateOrder(MouseEventArgs args, object data)
        {
            FirmaCurierat.Models.FirmaCurierat.Clienti data2 = new Models.FirmaCurierat.Clienti();
            // drivers2.RemoveAll(d => d.id_sofer == data2.id_sofer);
            data2 = (Models.FirmaCurierat.Clienti)data;
            UriHelper.NavigateTo($"edit-order/{data2.id_client}");
            //var dialogResult = await DialogService.OpenAsync<EditClientandOrder>("Edit Order", new Dictionary<string, object>() { { "client_id", data2.id_client} });
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
