using Carter.SirenNegotiator;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Carter.SirenNegotiator.Sample.Features.Actors;
using Carter.SirenNegotiator.Sample.Features.Home;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IActorProvider, ActorProvider>();
builder.Services.AddScoped<ISirenResponseGenerator, ActorResponseGenerator>();
builder.Services.AddScoped<IResponseNegotiator, SirenResponseNegotiator>();
builder.Services.AddCarter(configurator: c => {
    c.WithModule<HomeModule>();
    c.WithModule<ActorsModule>();
});

var app = builder.Build();

app.MapCarter();

app.Run();