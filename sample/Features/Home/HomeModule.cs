namespace Carter.SirenNegotiator.Sample.Features.Home;

using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

public class HomeModule : CarterModule
{
    public HomeModule()
    {
        After = async ctx =>
        {
            await Console.Out.WriteLineAsync("Catch you later!");
        };
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (HttpRequest req, HttpResponse res) =>
        {
            res.StatusCode = 409;
            await res.WriteAsync("There's no place like 127.0.0.1");
        });
    }
}