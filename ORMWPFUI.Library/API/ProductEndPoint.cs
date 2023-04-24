using ORMWPFUI.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUI.Library.API
{
    public class ProductEndPoint : IProductEndPoint
    {
        private IAPIHelper _anAPIHelper;

        public ProductEndPoint(IAPIHelper anAPIHelper)
        {
            _anAPIHelper = anAPIHelper;
        }
        public async Task<List<UIProductModel>> GetAll()
        {
            using (HttpResponseMessage response = await _anAPIHelper.GetHttpClient.GetAsync("/api/product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UIProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
