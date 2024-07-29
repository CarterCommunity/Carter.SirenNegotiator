namespace Carter.SirenNegotiator.Sample.Features.Actors;

using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FluentValidation;
using Carter.ModelBinding;
using System;
using System.Threading.Tasks;
public class ActorsModule : ICarterModule
{
    private readonly IActorProvider actorProvider;

    public ActorsModule(IActorProvider actorProvider)
    {
        this.actorProvider = actorProvider;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/actors", async (HttpRequest req, HttpResponse res) => {
            var people = actorProvider.Get();
            await res.Negotiate(people);
        });

        app.MapPost("/actors", async (HttpRequest req, HttpResponse res, Actor actor) =>
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

        app.MapGet("/actors/{id:int}", async (HttpRequest req, HttpResponse res, int id) =>
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

        app.MapPut("/actors/{id:int}", async (HttpRequest req, HttpResponse res, Actor actor, int id) =>
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

        app.MapDelete("/actors/{id:int}", (HttpRequest req, HttpResponse res, int id) =>
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
