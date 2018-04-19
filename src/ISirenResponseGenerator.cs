namespace Carter.SirenNegotiator
{
    using System;
    using System.Collections.Generic;

    public interface ISirenResponseGenerator
    {
        bool CanHandle(Type type);
        Siren Write(IEnumerable<object> data, Uri uri);
        Siren Write(object data, Uri uri);
    }
}