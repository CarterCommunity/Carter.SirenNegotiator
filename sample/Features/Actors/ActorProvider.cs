namespace Carter.SirenNegotiator.Sample.Features.Actors;

using System.Collections.Generic;
using System.Linq;

public class ActorProvider : IActorProvider
{
    private static IList<Actor> database = new[] { new Actor { Name = "Brad Pitt", Id = 1, Age = 51 }, new Actor { Name = "Jason Statham", Id = 2, Age = 43 } };

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