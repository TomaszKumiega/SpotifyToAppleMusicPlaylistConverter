using Autofac;
using SpotifyAPI.Web;
using SpotifyToAppleMusicPlaylistConverter.Clients;
using SpotifyToAppleMusicPlaylistConverter.Clients.Factories;
using SpotifyToAppleMusicPlaylistConverter.Configuration;

namespace SpotifyToAppleMusicPlaylistConverter.DependencyInjection
{
    internal static class DependencyInjectionConfigurationBuilder
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SpotifyClientFactory>().As<ISpotifyClientFactory>();    
            builder.RegisterType<SpotifyClient>().As<ISpotifyClient>();
            builder.RegisterType<ApplicationConfig>().AsSelf();
            builder.RegisterType<SpotifyClientWrapper>().AsSelf();

            return builder.Build();
        }
    }
}
