namespace Carter.SirenNegotiator.Sample
{
    using System;
    using Carter;
    using Microsoft.AspNetCore.Http;

    public class HomeModule : CarterModule
    {
        public HomeModule()
        {
            Get("/", async (req, res, routeData) =>
            {
                res.StatusCode = 409;
                await res.WriteAsync("There's no place like 127.0.0.1");
            });

            After = async ctx =>
            {
                await Console.Out.WriteLineAsync("Catch you later!");
            };
        }
    }
}
