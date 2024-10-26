using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace SpotifyToAppleMusicPlaylistConverter.Clients.Factories
{
    internal interface ISpotifyClientFactory
    {
        SpotifyClient Create(string clientId, string clientSecret, AuthorizationCodeResponse response, Uri uri);
    }
}