namespace Carter.SirenNegotiator.Sample
{
    using Carter;
    using CarterSample.Features.Actors;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActorProvider, ActorProvider>();
            services.AddScoped<ISirenDocumentWriter<Actor>, ActorDocumentWriter>();
            services.AddScoped<IResponseNegotiator, SirenResponseNegotiator>();
            services.AddCarter();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseCarter();
        }
    }
}
