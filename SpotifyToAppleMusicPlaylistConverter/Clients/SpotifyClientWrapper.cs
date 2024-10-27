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

        #region Login
        public async Task<SpotifyClientWrapper> Login()
        {
            _authServer = new EmbedIOAuthServer(_authorizationCallbackUri, 8000);
            await _authServer.Start();

            _authServer.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _authServer.ErrorReceived += OnErrorReceived!;

            var request = new LoginRequest(_authServer.BaseUri, _applicationConfig.SpotifyClientId, LoginRequest.ResponseType.Code)
            {
                Scope = [Scopes.UserReadEmail, Scopes.UserLibraryRead]
            };

            BrowserUtil.Open(request.ToUri());

            while (_spotifyClient == null)
            {
                await Task.Delay(1000);
            }

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
        #endregion

        public async Task<IAsyncEnumerable<SavedTrack>> GetUsersSavedTracks()
        {
            if (_spotifyClient == null)
            {
                throw new InvalidOperationException("Login first.");
            }

            var firstPage = await _spotifyClient.Library.GetTracks();

            return _spotifyClient.Paginate(firstPage);
        }
    }
}
