using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class RegisterComponent:ComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        public string username = "";
        public string password = "";
        public string message = "";
        public Models.FirmaCurierat.Clienti client;
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            client = new Models.FirmaCurierat.Clienti();
        }

        public async Task registerUser(MouseEventArgs args)
        {
            //verific sa nu am informatii necompletate
            if(client.nume!=""  && client. prenume != "" && username!="" && password!="")
            {
                SqlConnection scn = new SqlConnection();
                string ServerName = Environment.MachineName;

                string database = "CurieratVladProiect";
                string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);
                //inserez in baza de date cu info specificate de user
                scn.ConnectionString = ConnectionString;
                SqlCommand scmd = new SqlCommand("insert into clienti (nume,prenume,adresa,mail) values (@nam,@pre,@adr,@mail)", scn);
                scmd.Parameters.Clear();
                scmd.Parameters.AddWithValue("@nam", client.nume);
                scmd.Parameters.AddWithValue("@pre", client.prenume);
                scmd.Parameters.AddWithValue("@adr", client.adresa);
                scmd.Parameters.AddWithValue("@mail", client.mail);

                try
                {
                    scn.Open();
                    //executa comanda
                    scmd.ExecuteNonQuery();
                    //insereaza si in tabelul de login
                    SqlCommand scmd2= new SqlCommand("insert into login_database (username,password,id_client) values (@user,@pass,@id)", scn);
                    SqlCommand scmd3= new SqlCommand("select id_client from clienti where adresa=@adr", scn);
                    scmd3.Parameters.AddWithValue("@adr", client.adresa);
                    int id = (int)scmd3.ExecuteScalar();
                    scmd2.Parameters.AddWithValue("@user", username);
                    scmd2.Parameters.AddWithValue("@pass", password);
                    scmd2.Parameters.AddWithValue("@id", id);
                    scmd2.ExecuteNonQuery();
                    UriHelper.NavigateTo("/");

                }
                catch (Exception ex)
                {
                    throw;
                   
                    // MessageBox.Show("Can not open connection ! ");
                }
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"All field are required!");
            }
           



        }
    }
}
