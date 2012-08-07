using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Particles;

namespace Cardio.Phone.Shared
{
    public class GameCache
    {
        public List<DrawableGameObject> Cache { get; set; }

        public List<BlowEffectBox> BlowCache { get; set; }

        public void Clear()
        {
            Cache.Clear();
            BlowCache.Clear();
        }

        public void Add(DrawableGameObject obj)
        {
            Cache.Add(obj);
        }

        public void Remove(DrawableGameObject obj)
        {
            Cache.Remove(obj);
        }

        public GameCache()
        {
            Cache = new List<DrawableGameObject>();
            BlowCache = new List<BlowEffectBox>();
        }

        public void AddBlow(BlowType blowType, BlowEffect blow)
        {
            BlowCache.Add(new BlowEffectBox() { BlowType = blowType, BlowEffect = blow });
        }

        public BlowEffectBox GetBlowByType(BlowType blowType)
        {
            BlowEffectBox result = null;

            foreach (var b in BlowCache)
            {
                if (b.BlowType == blowType)
                {
                    result = b;
                    break;
                }
            }

            return result;
        }

        public BlowEffectBox GetWithRemoveBlowByType(BlowType blowType)
        {
            BlowEffectBox result = null;

            foreach (var b in BlowCache)
            {
                if (b.BlowType == blowType)
                {
                    result = b;
                    break;
                }
            }

            if (result != null)
            {
                BlowCache.Remove(result);
            }


            return result;
        }
    }
}
