using Autofac;

namespace ImdbMoviesConsoleApp
{
    public static class DependencyConfiguration
    {
        public static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MovieRepository>().As<IMovieRepository>();
            builder.RegisterType<DapperMovieRepository>().As<IMovieRepository>();
            return builder.Build();
        }
    }
}
