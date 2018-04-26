# Carter.SirenNegotiator
[![Build status](https://ci.appveyor.com/api/projects/status/wdmt5j8lcu68ryl3/branch/master?svg=true)](https://ci.appveyor.com/project/ritasker/carter-sirennegotiator/branch/master) 
[![NuGet](https://img.shields.io/nuget/v/Carter.SirenNegotiator.svg)](https://www.nuget.org/packages/Carter.SirenNegotiator/)

Carter.SirenNegotiator adds content negotiation for [Siren](https://github.com/kevinswiber/siren) requests to your [Carter](https://github.com/CarterCommunity/Carter) API.

## Installation

##### Packaage Manager
`PM> Install-Package Carter.SirenNegotiator`

##### .Net CLI
`$ dotnet add package Carter.SirenNegotiator`

## Usage 

Taken from the `sample` project.

#### Create a Response Generator

```c#
public class ActorResponseGenerator : ISirenResponseGenerator
{
    public bool CanHandle(Type type)
    {
        var listType = typeof(IEnumerable<Actor>);
        var classType = typeof(Actor);
        return classType.IsAssignableFrom(type) || listType.IsAssignableFrom(type);
    }

    public Siren Generate(object data, Uri uri)
    {
        return data is IEnumerable<Actor> 
            ? Generate((IEnumerable<Actor>) data, uri) 
            : Generate((Actor) data, uri);
    }

    private Siren Generate(IEnumerable<Actor> actors, Uri uri)
    {
        var doc = new Siren
        {
            @class = new [] { "collection" },
            entities = new List<Entity>(),
            properties = new { Count = actors.Count() }
        };

        foreach (var actor in actors)
        {
            var entity = new Entity
            {
                @class = new [] { nameof(Actor) },
                rel = new [] { "item" },
                properties = actor,
                links = new List<Link> { new Link { href = uri + "/" + actor.Id, rel = new [] { "self" } } }
            };

            doc.entities.Add (entity);
        }

        doc.actions = new List<Action> (new []{

            new Action
            {
                name = "create-actor",
                title = "Create Actor",
                method = "POST",
                href = uri.ToString(),
                type = "application/json",
                fields = new List<Field>(new[] {new Field {name = "name", type = "text"}, new Field{name = "age", type = "number"}})
            }
        });

        doc.links = new List<Link> { new Link { href = uri.ToString(), rel = new [] { "self" } } };

        return doc;
    }
    
    private Siren Generate(Actor actor, Uri uri)
    {
        return new Siren
        {
            @class = new [] { nameof(Actor) },
            properties = actor,
            links = new List<Link> { new Link { href = uri.ToString(), rel = new [] { "self" } } },
            actions = new List<Action> (new []{
                new Action
                {
                    name = "update-actor",
                    title = "Update Actor",
                    method = "PUT",
                    href = uri.ToString(),
                    type = "application/json",
                    fields = new List<Field>(new[] {new Field {name = "name", type = "text"}, new Field{name = "age", type = "number"}})
                },
                new Action
                {
                    name = "delete-actor",
                    title = "Delete Actor",
                    method = "DELETE",
                    href = uri.ToString()
                }
            })
        };
    }
}
```
This respose generator can handle a list of actors `IList<Actor>` or a single `Actor`.

#### Register Your Response Generator

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IActorProvider, ActorProvider>();
    services.AddScoped<ISirenResponseGenerator, ActorResponseGenerator>();
    
    services.AddCarter();
}
``` 

#### Create Your Module

```c#
public class ActorsModule : CarterModule
{
    public ActorsModule(IActorProvider actorProvider)
    {
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
    }
}
```

#### Send a Request

```bash
$ curl -H "Accept: application/vnd.siren+json" http://localhost:5000/actors/1
```
```json
{
  "class": [
    "Actor"
  ],
  "properties": {
    "Name": "Brad Pitt",
    "Id": 1,
    "Age": 51
  },
  "actions": [
    {
      "name": "update-actor",
      "title": "Update Actor",
      "method": "PUT",
      "href": "http://localhost:5000/actors/1",
      "type": "application/json",
      "fields": [
        {
          "name": "name",
          "type": "text"
        },
        {
          "name": "age",
          "type": "number"
        }
      ]
    },
    {
      "name": "delete-actor",
      "title": "Delete Actor",
      "method": "DELETE",
      "href": "http://localhost:5000/actors/1"
    }
  ],
  "links": [
    {
      "rel": [
        "self"
      ],
      "href": "http://localhost:5000/actors/1"
    }
  ]
}
```

## Contributing

Carter.SirenNegotiator is a community project. We invite your participation through issues and pull requests!

<a href="https://join.slack.com/t/cartercommunity/shared_invite/enQtMzQwNjIwODcwMTMxLWQwMjk5NDFlYWI3Yzg5Y2M4ODNmOTkwMzA2YjkxNmE0YjI3YWU4MjU2ZjI2NmQwMmE4NjVlODBlM2RlMDI1ZmY"><img src="./slack.svg" width="140px"/></a>

## License

Carter.SirenNegotiator is licensed under MIT. Refer to [LICENSE](https://github.com/CarterCommunity/Carter.SirenNegotiator/blob/master/LICENSE) for more information.