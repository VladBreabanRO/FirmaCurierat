using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class AddOrdersAndClientsComponent:ComponentBase
    {
        private readonly IJSRuntime jsRuntime;
        protected Models.FirmaCurierat.Clienti client;
        protected Models.FirmaCurierat.Comenzi comanda;
        protected List<Models.FirmaCurierat.TipComenzi> tip;
        protected Models.FirmaCurierat.TipComenzi tip_selected;
        protected List<Models.FirmaCurierat.Dispeceri> coordsList;
        protected DataBaseManagement.DataManagement dataHelper;
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime
        {
            get; set;
        }
        public List<Models.FirmaCurierat.Soferi> soferi;
        
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            soferi = new List<Models.FirmaCurierat.Soferi>();
            client = new Models.FirmaCurierat.Clienti();
            comanda = new Models.FirmaCurierat.Comenzi();
            tip = new List<Models.FirmaCurierat.TipComenzi>();
            coordsList = new List<Models.FirmaCurierat.Dispeceri>();
            tip_selected = new Models.FirmaCurierat.TipComenzi();
            // SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
            //lista de dispeceri pentur a putea atribuii comenzii
            coordsList = new List<Models.FirmaCurierat.Dispeceri>();
            string sqlCommand = "select * from  dispeceri";
            dataHelper = new DataBaseManagement.DataManagement();
            coordsList = await dataHelper.LoadData<Models.FirmaCurierat.Dispeceri, dynamic>(sqlCommand, new { }, ConnectionString);
            //ce tipuri de comenzi am
            sqlCommand = "select * from tip_comenzi";
            tip = await dataHelper.LoadData<Models.FirmaCurierat.TipComenzi, dynamic>(sqlCommand, new { }, ConnectionString);
            //soferii
            sqlCommand = "select * from soferi";
            soferi = await dataHelper.LoadData<Models.FirmaCurierat.Soferi, dynamic>(sqlCommand, new { }, ConnectionString);

        }
        protected async Task<bool> verifyAllField()
        {
            //verific sa fie completat tot
            if (client.nume == null || client.prenume == null || client.adresa == null || client.oras == null || client.mail == null || comanda.data_livrare == null || comanda.awb == null || comanda.id_dispecer == null || tip_selected.id_tip == null)
                return false;
            else return true;
        }
        protected async Task registerOrder(MouseEventArgs args)
        {
            try
            {
                var ok = await verifyAllField();
                if (ok == false)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"All field are required!");
                    return;
                }
                SqlConnection scn = new SqlConnection();
                string ServerName = Environment.MachineName;
                //connection string general
                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
                scn.ConnectionString = ConnectionString;
                //insereaza
                SqlCommand scmd = new SqlCommand("insert into clienti (nume,prenume,adresa,mail,oras) values (@nam,@pre,@adr,@mail,@o); SELECT SCOPE_IDENTITY()", scn);
              scmd.Parameters.Clear();

                scmd.Parameters.AddWithValue("@nam", client.nume);
                scmd.Parameters.AddWithValue("@pre", client.prenume);
                //scmd.Parameters.AddWithValue("@an", driver.an_angajare);
                scmd.Parameters.AddWithValue("@adr", client.adresa);
                scmd.Parameters.AddWithValue("@mail", client.mail);
                scmd.Parameters.AddWithValue("@o", client.oras);
                scn.Open();
                int inserted_Client = Convert.ToInt32(scmd.ExecuteScalar());
                scmd = new SqlCommand("insert into comenzi (data_livrare,awb,id_dispecer,id_client,id_sofer) values (@date,@awb,@id1,@id2,@id3);SELECT SCOPE_IDENTITY()", scn);
                scmd.Parameters.AddWithValue("@date", comanda.data_livrare);
                scmd.Parameters.AddWithValue("@awb", comanda.awb);
                //scmd.Parameters.AddWithValue("@an", driver.an_angajare);
                scmd.Parameters.AddWithValue("@id1", comanda.id_dispecer);
                scmd.Parameters.AddWithValue("@id2", inserted_Client);
                scmd.Parameters.AddWithValue("@id3", comanda.id_sofer);
                int inserted_Order = Convert.ToInt32(scmd.ExecuteScalar());
                scmd = new SqlCommand("insert into tipul_comenzii (id_comanda, id_tip) values (@id1, @id2)", scn);
                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@id1", inserted_Order);
                scmd.Parameters.AddWithValue("@id2", tip_selected.id_tip);

                scmd.ExecuteNonQuery();
                //notifica
                NotificationService.Notify(NotificationSeverity.Success, $"Order added!");
                //return la pagina
                UriHelper.NavigateTo("/clientsandOrders");
            }
            catch(Exception e )
            {
                throw;
            }
        
        }
        protected async Task goBack(MouseEventArgs args)
        {
            UriHelper.NavigateTo("/clientsandOrders");
        }
        protected async Task<string> generateRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[20];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        protected async Task generateAwb(MouseEventArgs args)
        {
            try
            {
                comanda.awb = "";
                string myValue = await this.generateRandomString();
                
                var cp = new GetCaretPosition(JSRuntime);
                await cp.InsertAtCursor("generateAwb", myValue);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
