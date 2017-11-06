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
            typeof(T).GetElementType().GetConstructors().Where(x => x.GetParameters().Length == 1).FirstOrDefault() :
            typeof(T).GetConstructors().Where(x => x.GetParameters().Length == 1).FirstOrDefault();

        private static readonly Deserializer<T> defaultInstance = new Deserializer<T>();

        public static Deserializer<T> Default => defaultInstance;

        public T Deserialize(string text)
        {
            if (CopyConstructor != null)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(text);
                if (obj is JArray jarr)
                {
                    if (typeof(T).IsArray)
                    {
                        return (T)ConvertToArrayMethod.Value.Invoke(null, new object[] { jarr });
                    }
                    else
                    {
                        throw new NotSupportedException($"{typeof(T).Name} is not supported :(");
                    }
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
