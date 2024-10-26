using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace SpotifyToAppleMusicPlaylistConverter.Clients.Factories
{
    internal class SpotifyClientFactory : ISpotifyClientFactory
    {
        public SpotifyClient Create(string clientId, string clientSecret, AuthorizationCodeResponse response, Uri uri)
        {
            SpotifyClientConfig clientConfig = SpotifyClientConfig.CreateDefault();
            AuthorizationCodeTokenRequest credentialsRequest = new(clientId, clientSecret, response.Code, uri);
            OAuthClient oauthClient = new(clientConfig);
            AuthorizationCodeTokenResponse authorizationTokenResponse = oauthClient
                .RequestToken(credentialsRequest)
                .Result;

            return new SpotifyClient(clientConfig.WithToken(authorizationTokenResponse.AccessToken));
        }
    }
}
