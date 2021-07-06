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
    public partial class LogInComponent:ComponentBase
    {
        public string userName
        {
            get;
            set;

        }
        public string Password
        {
            get;
            set;

        }
        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        public string username = "";
        public string password = "";
        public string message = "";

        public bool IsVisible = false;

        public void CloseDialog()
        {
            this.IsVisible = false;
        }
        public async Task ValidateUser(MouseEventArgs args)
        {          
            
            SqlConnection scn = new SqlConnection();
            string ServerName = Environment.MachineName;

            string database = "CurieratVladProiect";
            string ConnectionString = String.Format(@"Server={0}\SQLEXPRESS;Initial Catalog={1};
                                               Integrated Security = SSPI", ServerName, database);

            scn.ConnectionString = ConnectionString;
            //verific sa vad daca exista un utilizator cu username si password specificat
            SqlCommand scmd = new SqlCommand("select count (*) as cnt from login_database where username=@usr and password=@pwd", scn);
            scmd.Parameters.Clear();
            scmd.Parameters.AddWithValue("@usr", username);
            scmd.Parameters.AddWithValue("@pwd", password);
       
            try
            {
                scn.Open();
               if( scmd.ExecuteScalar().ToString() == "1" )
                {
                    //daca e admit merg la pagina si layout-l de administrare
                    if(username == "admin")
                    {
                        UriHelper.NavigateTo("/quickactions");
                    }
                    else
                    {
                        //daca nu, merg la layout-ul de user
                        UriHelper.NavigateTo("/user");
                    }
                   
                }
                else
                {
                    username = "";
                    password = "";
                    message = "Wrong username or password!";
                    IsVisible = true;
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Invalid username and password!");
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
                // MessageBox.Show("Can not open connection ! ");
            }
          



        }
        public string usernameNew = "";
        public string passNew = "";
        public async Task registerUser(MouseEventArgs args)
        {

            UriHelper.NavigateTo("/register");


        }
    }
    
}

