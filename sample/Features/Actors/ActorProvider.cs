using System.Collections.Generic;
using System.Linq;

namespace Carter.SirenNegotiator.Sample.Features.Actors;

public class ActorProvider : IActorProvider
{
    private static IList<Actor> database =
    [
        new()
        {
            Id = 1,
            Name = "Brad Pitt",
            Age = 51
        },
        new()
        {
            Id = 2,
            Name = "Jason Statham",
            Age = 43
        }
    ];

    public IEnumerable<Actor> Get()
    {
        return database;
    }

    public Actor Get(int id)
    {
        return database.First(x => x.Id == id);
    }

    public void Add(Actor actor)
    {
        actor.Id = database.Max(x => x.Id) + 1;
        database.Add(actor);
    }

    public void Update(Actor actor)
    {
        var actorRef = database.First(x => x.Id == actor.Id);
        actorRef.Age = actor.Age;
        actorRef.Name = actor.Name;
    }

    public void Delete(Actor actor)
    {
        database.Remove(actor);
    }
}