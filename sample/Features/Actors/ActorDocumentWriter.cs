namespace CarterSample.Features.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Carter.SirenNegotiator;
    using Action = Carter.SirenNegotiator.Action;

    public class ActorDocumentWriter : ISirenDocumentWriter<Actor>
    {
        public Siren Write(IEnumerable<Actor> data, Uri uri)
        {
            var doc = new Siren
            {
                @class = new [] { "collection" },
                entities = new List<Entity> (),
                properties = new { Count = data.Count() }
            };

            foreach (var actor in data)
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

        public Siren Write(Actor data, Uri uri)
        {
            return new Siren
            {
                @class = new [] { nameof(Actor) },
                properties = data,
                links = new List<Link> { new Link { href = uri + "/" + data.Id, rel = new [] { "self" } } },
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
    }
}