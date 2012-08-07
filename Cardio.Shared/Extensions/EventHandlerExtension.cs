using System;

namespace Cardio.UI.Extensions
{
    public static class EventHandlerExtension
    {
        public static void Fire(this EventHandler eventToFire, object sender,  Func<EventArgs> createArgs)
        {
            var handler = eventToFire;
            if (handler != null)
            {
                handler(sender, createArgs());
            }
        }

        public static void Fire<T>(this EventHandler<T> eventToFire, object sender, Func<T> createArgs) where T: EventArgs
        {
            var handler = eventToFire;
            if (handler != null)
            {
                handler(sender, createArgs());
            }
        }
    }
}