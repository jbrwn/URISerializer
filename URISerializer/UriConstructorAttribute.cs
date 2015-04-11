using System;

namespace URISerializer
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class UriConstructorAttribute : Attribute
    {
    }
}
