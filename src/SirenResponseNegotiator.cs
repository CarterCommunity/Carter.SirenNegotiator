namespace Carter.SirenNegotiator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using Response;

    public class SirenResponseNegotiator : IResponseNegotiator
    {
        private readonly IServiceProvider serviceProvider;

        public SirenResponseNegotiator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        
        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return IsExactMatch(accept.MediaType.ToString()) 
                   || IsPartialMatch(accept.SubType.ToString());
        }

        public async Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            var handlerType = typeof(ISirenDocumentWriter<>).MakeGenericType(model.GetType());
            var docWriter = serviceProvider.GetService(handlerType);
            
            var sirenRes = docWriter.Write(model, new Uri(req.Path.ToString()));  // Hmmmm! Can't resolve the type properly

            await res.AsJson(sirenRes);
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