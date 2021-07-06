using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Radzen;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class EditClientandOrderComponent : ComponentBase
    {
        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Parameter]
        public dynamic client_id { get; set; }
        protected Models.FirmaCurierat.Clienti client;
        protected Models.FirmaCurierat.Comenzi comanda;
        protected List<Models.FirmaCurierat.TipComenzi> tip;
        protected List<Models.FirmaCurierat.Comenzi> comenzi;
        protected Models.FirmaCurierat.TipComenzi tip_selected;
        protected List<Models.FirmaCurierat.Dispeceri> coordsList;
        protected DataBaseManagement.DataManagement dataHelper;
        public List<Models.FirmaCurierat.Clienti> clients
        {
            get;
            set;
        }
        protected async Task loadClientsandOrders()
        {
            comenzi = new List<Models.FirmaCurierat.Comenzi>();
            dataHelper = new DataBaseManagement.DataManagement();
            clients = new List<Models.FirmaCurierat.Clienti>();
            client = new Models.FirmaCurierat.Clienti();
            SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            scn.ConnectionString = ConnectionString;
            SqlCommand scmd = new SqlCommand("select * from  clienti where id_client = @id", scn);
            scmd.Parameters.Clear();

            scmd.Parameters.AddWithValue("@id", client_id);

            scn.Open();
            // var inserted_Client = (Models.FirmaCurierat.Clienti)scmd.ExecuteScalar();
            using (SqlDataReader reader = scmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    client.id_client = (int)reader["id_client"];
                    client.mail = (string)reader["mail"];
                    client.nume = (string)reader["nume"];
                    client.prenume = (string)reader["prenume"];
                    client.adresa = (string)reader["adresa"];
                   
                }
            }
            // SqlConnection scn = new SqlConnection();
            // scn.ConnectionString = @"Data Source=DESKTOP-I3NIEPL\SQLEXPRESS;Initial Catalog=login_database;database=CurieratVladProiect;integrated security=SSPI";
           // string ConnectionString = @"Data Source=DESKTOP-I3NIEPL\SQLEXPRESS;Initial Catalog=login_database;database=CurieratVladProiect;integrated security=SSPI";

            comanda = new Models.FirmaCurierat.Comenzi();
            scmd = new SqlCommand("select * from  comenzi where id_client = @id", scn);
            scmd.Parameters.Clear();

            scmd.Parameters.AddWithValue("@id", client_id);

            using (SqlDataReader reader = scmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    comanda.awb = (string)reader["awb"];
                    comanda.data_livrare = (string)reader["data_livrare"];
                    comanda.id_client = (int)reader["id_client"];
                    comanda.id_comanda = (int)reader["id_comanda"];
                    comanda.id_dispecer = (int)reader["id_dispecer"];

                }
            }

            comanda = comenzi.Where(i => i.id_client == Convert.ToInt32(client_id)).FirstOrDefault();
        }
        protected async Task loadLists()
        {
            dataHelper = new DataBaseManagement.DataManagement();
            SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            scn.ConnectionString = ConnectionString;
            coordsList = new List<Models.FirmaCurierat.Dispeceri>();
            string sqlCommand = "select * from  dispeceri";

            SqlCommand scmd = new SqlCommand("select * from  dispeceri", scn);
            scmd.Parameters.Clear();
            scn.Open();
            var dr = scmd.ExecuteReader();
            while (dr.Read())
            {
                Models.FirmaCurierat.Dispeceri temp = new Models.FirmaCurierat.Dispeceri();
                temp.id_dispecer = (int)dr["id_dispecer"];
                temp.nume_dispecer = (string)dr["nume_dispecer"];
                coordsList.Add(temp);
            }
            sqlCommand = "select * from tip_comenzi";
            tip = new List<Models.FirmaCurierat.TipComenzi>();
            scmd = new SqlCommand("select * from tip_comenzi", scn);
            dr.Close();
            scmd.Parameters.Clear();
             dr = scmd.ExecuteReader();
          
            while(dr.Read())
            {
                Models.FirmaCurierat.TipComenzi temp = new Models.FirmaCurierat.TipComenzi();
                temp.id_tip = (int)dr["id_tip"];
                temp.specificatii = (string)dr["specificatii"];
                temp.tip = (string)dr["tip"];
            }

        }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            try
            {
                await loadClientsandOrders();
              

            }
            catch (Exception e)
            {
                throw;
            }


        }
        protected async Task update(MouseEventArgs args)
        {
            try
            {
                //functie de update
                string ServerName = Environment.MachineName;

                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
                SqlConnection scn = new SqlConnection();
                scn.ConnectionString = ConnectionString;
                SqlCommand scmd = new SqlCommand("update clienti SET nume = @nam, prenume = @pre, adresa= @adr, mail = @mail where id_client = @id", scn);
                scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@nam", client.nume);
                scmd.Parameters.AddWithValue("@pre", client.prenume);
                //scmd.Parameters.AddWithValue("@an", driver.an_angajare);
                scmd.Parameters.AddWithValue("@adr", client.adresa);
                scmd.Parameters.AddWithValue("@mail", client.mail);
                scmd.Parameters.AddWithValue("@id", client_id);
                scn.Open();
                int inserted_Client = Convert.ToInt32(scmd.ExecuteScalar());
                NotificationService.Notify(NotificationSeverity.Success, $"Client  edited!");
                UriHelper.NavigateTo("/clientsandOrders");
            } catch(Exception e )
            {
                throw;
            }
       
        }
        protected async Task goBack(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/clientsandOrders");
        }
    }
}
