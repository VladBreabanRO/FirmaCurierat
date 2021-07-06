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
    public class CounterComponent:ComponentBase
    {
        public List<Models.FirmaCurierat.Soferi> drivers;

        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        public NavigationManager UriHelper { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            dataHelper = new DataBaseManagement.DataManagement();
            // SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

            drivers = new List<FirmaCurierat.Models.FirmaCurierat.Soferi>();
            string sqlCommand = "select * from  soferi";
            drivers = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.Soferi, dynamic>(sqlCommand, new { }, ConnectionString);
        }

     protected Syncfusion.Blazor.Grids.SfGrid<FirmaCurierat.Models.FirmaCurierat.Soferi> grid0
        {
            get;
            set;
        }
        //functie pentru stergere sofer din baza de date
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, object data)
        {
            List<Models.FirmaCurierat.Soferi> drivers2 = new List<Models.FirmaCurierat.Soferi>();
            FirmaCurierat.Models.FirmaCurierat.Soferi data2 = new Models.FirmaCurierat.Soferi();

            data2 = (Models.FirmaCurierat.Soferi)data;
            drivers2 = drivers;
            try
            {
                if (await DialogService.Confirm("Do you want to delete this record?") == true)
                {
                  
                    drivers = new List<Models.FirmaCurierat.Soferi>();
                 

                    SqlConnection scn = new SqlConnection();
                    string ServerName = Environment.MachineName;

                    string database = "CurieratVladProiect";
                    string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

                    scn.ConnectionString = ConnectionString;
                    SqlCommand scmd = new SqlCommand("delete from soferi where id_sofer =  @id", scn);
                    scmd.Parameters.Clear();
                    scmd.Parameters.AddWithValue("@id", data2.id_sofer);
                    scn.Open();
                    scmd.ExecuteNonQuery();
                  
                
                }
            }
            catch (System.Exception rlvMailerDeleteMailException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Driver has orders in delivery!");
                return;
            }
            drivers2.RemoveAll(d => d.id_sofer == data2.id_sofer);
            drivers = drivers2;
            grid0.Refresh();
            this.StateHasChanged();
        }
        protected async Task goToAdd(MouseEventArgs args)
        {
           
            UriHelper.NavigateTo("/addDriver");
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
