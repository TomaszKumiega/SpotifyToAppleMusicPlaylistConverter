using Autofac;
using SpotifyAPI.Web;
using SpotifyToAppleMusicPlaylistConverter.Configuration;

namespace SpotifyToAppleMusicPlaylistConverter.DependencyInjection
{
    internal static class DependencyInjectionConfigurationBuilder
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SpotifyClient>().As<ISpotifyClient>();
            builder.RegisterType<ApplicationConfig>().AsSelf();

            return builder.Build();
        }
    }
}
