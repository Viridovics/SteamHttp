using SteamHttp.Client;
using SteamHttp.NewtonsoftJson.Configuration;

namespace SteamHttp.NewtonsoftJson
{
    public class SteamHttpNewtonsoftJsonClient : SteamHttpClient
    {
        public SteamHttpNewtonsoftJsonClient():base(new NewtonsoftJsonSteamHttpContentConfiguration())
        {}
    }
}