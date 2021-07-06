using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaCurierat.Pages
{ // clasa pentru inserare awb la cursor
    public class GetCaretPosition
    {
        private readonly IJSRuntime jsRuntime;
        public GetCaretPosition(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }
        public ValueTask<object> InsertAtCursor(string elementID, string myValue)
        {

            try
            {
                return jsRuntime.InvokeAsync<object>(
               "MyJSFunctions.InsertAtCursor", elementID, myValue);
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
