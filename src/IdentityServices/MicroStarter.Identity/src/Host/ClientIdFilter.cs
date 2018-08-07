using System;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Host;

namespace Host
{
    public class ClientIdFilter : IActionFilter
    {
        public ClientIdFilter(ClientSelector clientSelector)
        {
            _clientSelector = clientSelector;
        }

        public string Client_id = "none";
        private readonly ClientSelector _clientSelector;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var query = context.HttpContext.Request.Query;
            var exists = query.TryGetValue("client_id", out StringValues culture);

            if (!exists)
            {
                exists = query.TryGetValue("returnUrl", out StringValues requesturl);

                if (exists)
                {
                    var request = requesturl.ToArray()[0];
                    Uri uri = new Uri("http://faketopreventexception" + request);
                    var query1 = QueryHelpers.ParseQuery(uri.Query);
                    var client_id = query1.FirstOrDefault(t => t.Key == "client_id").Value;

                    _clientSelector.SelectedClient = client_id.ToString();
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
