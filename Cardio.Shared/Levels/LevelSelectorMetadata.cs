using System.Collections.Generic;

namespace Cardio.UI.Levels
{
    public class LevelSelectorMetadata
    {
        public int ColumnsCount
        {
            get; set;
        }

        public int ThumbnailHeight
        { 
            get; set;
        }

        public int ThumbnailWidth
        {
            get; set;
        }

        public List<LevelThumbailMetadata> Levels
        {
            get; set;
        }

        public LevelSelectorMetadata()
        {
            Levels = new List<LevelThumbailMetadata>();
        }
    }
}
