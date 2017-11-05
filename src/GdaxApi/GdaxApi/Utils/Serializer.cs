namespace GdaxApi.Utils
{
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
            return Deserializer<T>.Default.Deserialize(text);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
