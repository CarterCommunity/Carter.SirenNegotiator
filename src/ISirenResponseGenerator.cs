namespace Carter.SirenNegotiator
{
    using System;

    public interface ISirenResponseGenerator
    {
        bool CanHandle(Type type);
        Siren Generate(object data, Uri uri);
    }
}
