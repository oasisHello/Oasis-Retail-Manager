using ORMWPFUI.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.API
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private IAPIHelper _anAPIHelper;

        public SaleEndPoint(IAPIHelper anAPIHelper)
        {
            _anAPIHelper = anAPIHelper;
        }

        public async Task PostSale(UISaleModel sale)
        {
            using (HttpResponseMessage response = await _anAPIHelper.GetHttpClient.PostAsJsonAsync("/api/sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }
    }
}
