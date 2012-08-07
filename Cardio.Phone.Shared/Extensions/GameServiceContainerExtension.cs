using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Extensions
{
    public static class GameServiceContainerExtension
    {
        public static T GetService<T>(this GameServiceContainer serviceContainer)
        {
            return (T)serviceContainer.GetService(typeof(T));
        }

        public static void AddService<T>(this GameServiceContainer serviceContainer, T service)
        {
            serviceContainer.AddService(typeof(T), service);
        }

        public static void RemoveService<T>(this GameServiceContainer serviceContainer)
        {
            serviceContainer.RemoveService(typeof(T));
        }
    }
}
