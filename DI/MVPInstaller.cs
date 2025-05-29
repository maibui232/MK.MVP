namespace MK.MVP.DI
{
    using MK.DependencyInjection;

    public static class MVPInstaller
    {
        public static void InstallMVP(this IBuilder builder)
        {
            builder.Register<ViewUIService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}