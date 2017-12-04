namespace GdaxApi.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Deserializer<T>
    {
        private static readonly Lazy<MethodInfo> ConvertToArrayMethod = new Lazy<MethodInfo>(() => typeof(Deserializer<T>)
            .GetMethod("ConvertToArray", BindingFlags.Static | BindingFlags.NonPublic)
            .MakeGenericMethod(typeof(T).GetElementType()));

        private static readonly ConstructorInfo CopyConstructor = typeof(T).IsArray ?
            Array.Find(typeof(T).GetElementType().GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic), x => x.GetParameters().Length == 1) :
            Array.Find(typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic), x => x.GetParameters().Length == 1);

        private static readonly Deserializer<T> defaultInstance = new Deserializer<T>();

        public static Deserializer<T> Default => defaultInstance;

        public T Deserialize(string text)
        {
            if (CopyConstructor != null)
            {
                var obj = JsonConvert.DeserializeObject<JToken>(text);
                if (typeof(T).IsArray)
                {
                    return (T)ConvertToArrayMethod.Value.Invoke(null, new object[] { obj });
                }

                return (T)CopyConstructor.Invoke(new object[] { obj });
            }

            return JsonConvert.DeserializeObject<T>(text);
        }

        private static IEnumerable<U> ConvertToEnumerable<U>(JArray jarr)
        {
            foreach (var item in jarr)
            {
                yield return (U)CopyConstructor.Invoke(new object[] { item });
            }
        }

        private static U[] ConvertToArray<U>(JArray jarr)
        {
            return ConvertToEnumerable<U>(jarr).ToArray();
        }
    }
}
