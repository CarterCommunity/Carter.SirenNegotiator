namespace Carter.SirenNegotiator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Net.Http.Headers;
    using Response;

    public class SirenResponseNegotiator : IResponseNegotiator
    {
        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return IsExactMatch(accept.MediaType.ToString()) 
                   || IsPartialMatch(accept.SubType.ToString());
        }

        public async Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            var responseGenerators = req.HttpContext.RequestServices.GetServices(typeof(ISirenResponseGenerator)) as IEnumerable<ISirenResponseGenerator>;
            var sirenResponseGenerator = responseGenerators.First(x => x.CanHandle(model.GetType()));
            var response = sirenResponseGenerator.Write(model, new Uri(req.Path.ToString()));

            await res.AsJson(response);
        }
        
        private bool IsExactMatch(string mediaType)
        {
            return mediaType.Contains("application/vnd.siren+json");
        }

        private bool IsPartialMatch(string subType)
        {
            return subType.StartsWith("vnd", StringComparison.OrdinalIgnoreCase) 
                   && subType.EndsWith("siren+json", StringComparison.OrdinalIgnoreCase);
        }
    }
}