using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ORMDataManager.Controllers
{
    [Authorize]//Note: If you want to access the endpoint in this controller, you have to be loggin.
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();//Note: there is probably more than one loggin,
                                                                          //I need to know you more precisely. --ESP03,Verify your user id.
            return new string[] { "value1", "value2",userId};
        }
        
    }
}
