using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyToAppleMusicPlaylistConverter.Clients.Factories;
using SpotifyToAppleMusicPlaylistConverter.Configuration;

namespace SpotifyToAppleMusicPlaylistConverter.Clients
{
    internal class SpotifyClientWrapper(
        ApplicationConfig applicationConfig,
        ISpotifyClientFactory spotifyClientFactory)
    {
        private static Uri _authorizationCallbackUri = new("http://localhost:8000/spotifyAuth/");

        private EmbedIOAuthServer? _authServer;
        private SpotifyClient? _spotifyClient;
        private readonly ApplicationConfig _applicationConfig = applicationConfig;
        private readonly ISpotifyClientFactory _spotifyClientFactory = spotifyClientFactory;

        public async Task<SpotifyClientWrapper> Login()
        {
            _authServer = new EmbedIOAuthServer(_authorizationCallbackUri, 8000);
            await _authServer.Start();

            _authServer.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _authServer.ErrorReceived += OnErrorReceived!;

            return this;
        }

        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            InitializeSpotifyClient(response);
            await _authServer!.Stop();
        }

        private async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Authorization error: {error}");
            await _authServer!.Stop();
        }

        private void InitializeSpotifyClient(AuthorizationCodeResponse response)
        {
            _spotifyClient = _spotifyClientFactory.Create(_applicationConfig.SpotifyClientId, _applicationConfig.SpotifyClientSecret, response, _authorizationCallbackUri);
        }
    }
}
