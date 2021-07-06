using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{
    public class UserComponent : ComponentBase
    {
        public string insertedAwb;
        [Inject]
        protected DialogService DialogService { get; set; }
        public async Task goToInfo(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<InfoAboutCom>("Order Information", new Dictionary<string, object>() { { "awb", insertedAwb } });
        }
    }
}
