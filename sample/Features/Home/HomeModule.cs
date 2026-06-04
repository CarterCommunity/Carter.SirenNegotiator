using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Carter.SirenNegotiator.Sample.Features.Home;

public class HomeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (HttpRequest req, HttpResponse res) =>
        {
            res.StatusCode = 409;
            await res.WriteAsync("There's no place like 127.0.0.1");
        });
    }
}