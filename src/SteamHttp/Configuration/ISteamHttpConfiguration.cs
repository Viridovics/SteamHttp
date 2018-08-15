
namespace SteamHttp.Configuration
{
    public interface ISteamHttpContentConfiguration
    {
        string ContentType { get; }

        T DeserializeObject<T>(string serializedObject);

        string SerializeObject<T>(T pocoObject);
    }
}
