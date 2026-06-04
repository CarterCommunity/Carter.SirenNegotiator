using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Carter.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Carter.SirenNegotiator.Sample.Features.Actors;

public class ActorsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/actors", async (HttpRequest req, HttpResponse res, IActorProvider actorProvider) => {
            var people = actorProvider.Get();
            await res.Negotiate(people);
        });

        app.MapPost("/actors", async (HttpRequest req, HttpResponse res, Actor actor, IActorProvider actorProvider) =>
        {
            var validationResult = req.Validate(actor);

            if (!validationResult.IsValid)
            {
                res.StatusCode = 422;
                await res.Negotiate(validationResult.GetFormattedErrors());
                return;
            }

            actorProvider.Add(actor);

            res.StatusCode = 201;
            await res.Negotiate(actor);
        });

        app.MapGet("/actors/{id:int}", async (HttpRequest req, HttpResponse res, int id, IActorProvider actorProvider) =>
        {
            try
            {
                var person = actorProvider.Get(id);
                await res.Negotiate(person);
            }
            catch (InvalidOperationException)
            {
                res.StatusCode = 404;
            }
        });

        app.MapPut("/actors/{id:int}", async (HttpRequest req, HttpResponse res, Actor actor, int id, IActorProvider actorProvider) =>
        {
            var validationResult = req.Validate(actor);

            if (!validationResult.IsValid)
            {
                res.StatusCode = 422;
                await res.Negotiate(validationResult.GetFormattedErrors());
                return;
            }

            try
            {
                actorProvider.Update(actor);

                res.StatusCode = 204;
            }
            catch (InvalidOperationException)
            {
                res.StatusCode = 404;
            }
        });

        app.MapDelete("/actors/{id:int}", (HttpRequest req, HttpResponse res, int id, IActorProvider actorProvider) =>
        {
            try
            {
                var actor = actorProvider.Get(id);
                actorProvider.Delete(actor);

                res.StatusCode = 204;
            }
            catch (InvalidOperationException)
            {
                res.StatusCode = 404;
            }

            return Task.CompletedTask;
        });
    }
}
