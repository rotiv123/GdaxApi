namespace GdaxApi.Utils
{
    using System;
    using GdaxApi.Exceptions;
    using Newtonsoft.Json;

    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string text);
    }

    public class Serializer : ISerializer
    {
        public T Deserialize<T>(string text)
        {
            try
            {
                return Deserializer<T>.Default.Deserialize(text);
            }
            catch (Exception ex)
            {
                throw new GdaxApiSerializationException($"Deserialization of type {typeof(T).FullName} failed", ex);
            }
        }

        public string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new GdaxApiSerializationException($"Serialization failed", ex);
            }
        }
    }
}
