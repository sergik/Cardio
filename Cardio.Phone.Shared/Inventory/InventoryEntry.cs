using Cardio.Phone.Shared.GUI;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Inventory
{
    public class InventoryEntry
    {
        private int _count;
        private Rectangle _renderingRectangle;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                CountText = _count.ToString();
            }
        }

        public ProgressBar ProgressBar { get; set; }

        public string CountText { get; set; }

        public InventoryItem Base { get; set; }

        public void Initialize(Game game, Rectangle renderingRectangle)
        {
            _renderingRectangle = renderingRectangle;
            ProgressBar = new ProgressBar(game, @"Textures\UI\Health", @"Textures\UI\HealthVessel")
                              {
                                  DestinationRectangle =
                                      new Rectangle(renderingRectangle.Left, renderingRectangle.Bottom,
                                                    renderingRectangle.Width, 12)
                              };
            ProgressBar.Max = Base.ReuseTime;
            ProgressBar.Initialize();
        }

        public void Update(GameTime time)
        {
            if(Base.IsReuseble)
            {
                if(Base.ColdownTime > 0)
                {
                    Base.ColdownTime -= (int) time.ElapsedGameTime.TotalMilliseconds;
                    if (Base.ColdownTime <= 0)
                    {
                        Base.ColdownTime = 0;
                        Base.IsEnabled = true;
                    }
                    ProgressBar.Value = Base.ColdownTime;
                    ProgressBar.Update(time);
                }
            }
        }

        public void Draw(GameTime time)
        {
            ProgressBar.Draw(time);
        }
    }
}