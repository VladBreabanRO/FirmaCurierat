using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class EmployeesComponent:ComponentBase
    {
        public List<Models.FirmaCurierat.Soferi> drivers;

      protected  DataBaseManagement.DataManagement dataHelper
        {
            get;
            set;
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            dataHelper = new DataBaseManagement.DataManagement();
           // SqlConnection scn = new SqlConnection();
            string ConnectionString = @"Data Source=DESKTOP-I3NIEPL\SQLEXPRESS;Initial Catalog=login_database;database=CurieratVladProiect;integrated security=SSPI";
            drivers = new List<FirmaCurierat.Models.FirmaCurierat.Soferi>();
            string sqlCommand = "select * from  soferi";
          //  scn.Open();
          drivers = await dataHelper.LoadData<FirmaCurierat.Models.FirmaCurierat.Soferi, dynamic>(sqlCommand, new { }, ConnectionString);
        }
    }
}
