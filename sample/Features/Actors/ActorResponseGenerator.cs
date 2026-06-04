using System;
using System.Collections.Generic;
using System.Linq;

namespace Carter.SirenNegotiator.Sample.Features.Actors;

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
        return data is IEnumerable<Actor> actors
            ? Generate(actors, uri)
            : Generate((Actor)data, uri);
    }

    private Siren Generate(IEnumerable<Actor> actors, Uri uri)
    {
        var actorArray = actors as Actor[] ?? actors.ToArray();
        var doc = new Siren
        {
            @class = ["collection"],
            entities = new List<Entity>(),
            properties = new { Count = actorArray.Count() }
        };

        foreach (var actor in actorArray)
        {
            var entity = new Entity
            {
                @class = [nameof(Actor)],
                rel = ["item"],
                properties = actor,
                links = [new Link { href = uri + "/" + actor.Id, rel = ["self"] }]
            };

            doc.entities.Add(entity);
        }

        doc.actions = new List<Action>([
            new Action
            {
                name = "create-actor",
                title = "Create Actor",
                method = "POST",
                href = uri.ToString(),
                type = "application/json",
                fields =
                [
                    new Field { name = "name", type = "text" },
                    new Field { name = "age", type = "number" }
                ]
            }
        ]);

        doc.links = [new Link { href = uri.ToString(), rel = ["self"] }];

        return doc;
    }

    private Siren Generate(Actor actor, Uri uri)
    {
        return new Siren
        {
            @class = [nameof(Actor)],
            properties = actor,
            links = [new Link { href = uri.ToString(), rel = ["self"] }],
            actions =
            [
                new Action
                {
                    name = "update-actor",
                    title = "Update Actor",
                    method = "PUT",
                    href = uri.ToString(),
                    type = "application/json",
                    fields =
                    [
                        new Field { name = "name", type = "text" }, 
                        new Field { name = "age", type = "number" }
                    ]
                },
                new Action
                {
                    name = "delete-actor",
                    title = "Delete Actor",
                    method = "DELETE",
                    href = uri.ToString()
                }
            ]
        };
    }
}