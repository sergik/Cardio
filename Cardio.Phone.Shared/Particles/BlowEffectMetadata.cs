using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardio.Phone.Shared.Particles
{
   
    public class BlowEffectMetadata
    {
        public int PartVelocity { get; set; }
        public PartsMovingLaw Law { get; set; }
        public string MovingPartsTextureName { get; set; }
        public int MovingPartsFramesCount { get; set; }
        public int MovingPartsDispertion { get; set; }
        public int MovingPartsIntervalBottom { get; set; }
        public int MovingPartsIntervalTop { get; set; }
        public int MaxMovingParts { get; set; }
        public int MinMovingParts { get; set; }
        public bool AreMovingPartsLooped { get; set; }
        public string CenterTextureName { get; set; }
        public int CenterFrameCount { get; set; }
        public int CenterInterval { get; set; }
        public bool IsCenterLooped { get; set; }
        public BlowType Type { get; set; }

    }
}
