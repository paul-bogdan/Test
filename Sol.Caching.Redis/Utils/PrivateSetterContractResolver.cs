using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sol.Caching.Redis.Utils;

public class PrivateSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (!property.Writable)
        {
            property.Writable = IsPropertyWithPrivateSetter(member);
        }

        return property;
    }
    private static bool IsPropertyWithPrivateSetter(MemberInfo member)
    {
        if (member is PropertyInfo property)
        {
            return property.GetSetMethod(true) != null;
        }

        return false;
    }
}