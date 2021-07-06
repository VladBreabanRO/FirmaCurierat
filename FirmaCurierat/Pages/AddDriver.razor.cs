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
    public class AddDriverComponent : ComponentBase
    {
        Models.FirmaCurierat.Soferi _mail;
        protected Models.FirmaCurierat.Soferi driver;
       
        protected Models.FirmaCurierat.Masini cars;

        protected Models.FirmaCurierat.Dispeceri coords;
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected List<Models.FirmaCurierat.Dispeceri> coordsList;
        protected List<Models.FirmaCurierat.Masini> carsList;
        protected DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }
        public int? id_masina;
        public int? id_coordonator;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {   //initializare liste
            driver = new Models.FirmaCurierat.Soferi();
            cars = new Models.FirmaCurierat.Masini();
            carsList = new List<Models.FirmaCurierat.Masini>();
            coordsList = new List<Models.FirmaCurierat.Dispeceri>();
            // SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;
            //connection string general
            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            //initializare liste
            coordsList = new List<Models.FirmaCurierat.Dispeceri>();
            carsList = new List<Models.FirmaCurierat.Masini>();
            //comanda
            string sqlCommand = "select * from  dispeceri";
            dataHelper = new DataBaseManagement.DataManagement();
            //  scn.Open();
            List<Models.FirmaCurierat.Masini> tempList = new List<Models.FirmaCurierat.Masini>();
            //apelare functie generala pentru obtinerea unei liste de elemente din baza de date
           coordsList = await dataHelper.LoadData<Models.FirmaCurierat.Dispeceri, dynamic>(sqlCommand, new { }, ConnectionString);
            sqlCommand = "select * from  masini";
            //tot apelare functie generala
          tempList = await dataHelper.LoadData<Models.FirmaCurierat.Masini, dynamic>(sqlCommand, new { }, ConnectionString);
            sqlCommand = "select id_masina from soferi";
            List<int> forSorting = new List<int>();
            forSorting = await dataHelper.LoadData<int, dynamic>(sqlCommand, new { }, ConnectionString);
            //aleg doar masinile care nu au asociate un sofer, relatia fiind de 1->1
            carsList = tempList.Where(i => forSorting.Contains(i.id_masina) == false).ToList();
        }

        public async Task register(MouseEventArgs args)
        {
            try
            {
                 if(driver.nume == null || driver.prenume == null || driver.id_masina == null || driver.id_dispecer == null)
                {
                    NotificationService.Notify(NotificationSeverity.Success, $"All field are required!");
                    return;
                }
                SqlConnection scn = new SqlConnection();
                string ServerName = Environment.MachineName;

                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

                scn.ConnectionString = ConnectionString;
                SqlCommand scmd = new SqlCommand("insert into soferi (nume,prenume,an_angajare,id_masina,id_dispecer) values (@nam,@pre,@an,@id1,@id2)", scn);
                scmd.Parameters.Clear();
                driver.an_angajare = 2020;
                scmd.Parameters.AddWithValue("@nam", driver.nume);
                scmd.Parameters.AddWithValue("@pre", driver.prenume);
                scmd.Parameters.AddWithValue("@an", driver.an_angajare);
                scmd.Parameters.AddWithValue("@id1", driver.id_masina);
                scmd.Parameters.AddWithValue("@id2", driver.id_dispecer);
                scn.Open();
                scmd.ExecuteNonQuery();
                NotificationService.Notify(NotificationSeverity.Success, $"Driver added!");
                Task.Delay(50);
                UriHelper.NavigateTo("/counter");
            } catch(Exception e)
            {
                throw;
            }
          
        }

        public async Task back(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/counter");
        }

    }
}
