namespace CarterSample.Features.Actors
{
    using System;
    using System.Threading.Tasks;
    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    public class ActorsModule : CarterModule
    {
        public ActorsModule(IActorProvider actorProvider)
        {
            Get("/actors", async (req, res, routeData) =>
            {
                var people = actorProvider.Get();
                await res.Negotiate(people);
            });
            
            Post("/actors", async (req, res, routeData) =>
            {
                var result = req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                actorProvider.Add(result.Data);

                res.StatusCode = 201;
                await res.Negotiate(result.Data);
            });

            Get("/actors/{id:int}", async (req, res, routeData) =>
            {
                try
                {
                    var person = actorProvider.Get(routeData.As<int>("id"));
                    await res.Negotiate(person);
                }
                catch (InvalidOperationException)
                {
                    res.StatusCode = 404;
                }
            });

            Put("/actors/{id:int}", async (req, res, routeData) =>
            {
                var result = req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                try
                {
                    actorProvider.Update(result.Data);

                    res.StatusCode = 204;
                }
                catch (InvalidOperationException)
                {
                    res.StatusCode = 404;
                }
            });
            
            Delete("/actors/{id:int}", (req, res, routeData) =>
            {
                try
                {
                    var actor = actorProvider.Get(routeData.As<int>("id"));
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
}
