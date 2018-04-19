namespace CarterSample.Features.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Carter.SirenNegotiator;
    using Action = Carter.SirenNegotiator.Action;

    public class ActorResponseGenerator : ISirenResponseGenerator
    {
        public Siren Write(IEnumerable<object> data, Uri uri)
        {
            var doc = new Siren
            {
                @class = new [] { "collection" },
                entities = new List<Entity> (),
                properties = new { Count = data.Count() }
            };

            foreach (var actor in data as IEnumerable<Actor>)
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

        public Siren Write(object data, Uri uri)
        {
            var actor = data as Actor;
            return new Siren
            {
                @class = new [] { nameof(Actor) },
                properties = data,
                links = new List<Link> { new Link { href = uri + "/" + actor.Id, rel = new [] { "self" } } },
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
                        method = "PUT",
                        href = uri.ToString()
                    }
                })
            };
        }

        public bool CanHandle(Type type)
        {
            var listType = typeof(IEnumerable<Actor>);
            return type is Actor || listType.IsAssignableFrom(type);
        }
    }
}