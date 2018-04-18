namespace CarterSample.Features.Actors
{
    using System.Collections.Generic;

    public interface IActorProvider
    {
        IEnumerable<Actor> Get();
        Actor Get(int id);
        void Add(Actor actor);
        void Update(Actor actor);
        void Delete(Actor actor);
    }
}