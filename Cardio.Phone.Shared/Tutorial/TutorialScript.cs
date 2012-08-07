namespace Cardio.Phone.Shared.Tutorial
{
    //public class TutorialScript: GameEntityScript
    //{
    //    protected GameState GameState { get; set; }

    //    protected override void OnStart(GameState state)
    //    {
    //        GameState = state;
    //        base.OnStart(state);
    //    }
    //}

    //public class TutorialScript01: TutorialScript
    //{
    //    private float _currentTime;
    //    private float _totalTime = 3000;

    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);
    //        GameState.Player.WorldPosition = new Vector2(-10000, 0);
    //        GameState.IsGodModeEnabled = true;
    //        GameState.Camera.Position = new Vector2(0, 0);

    //        GameState.Camera.FocusedAtOffset = new Vector2(0,0);
    //        GameState.Camera.FocusedAt = GameState.Level.Texts[0];
    //    }

    //    public override void Update(GameState state, GameTime gameTime)
    //    {
    //        var time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

    //        if (_currentTime >= _totalTime)
    //        {
    //            Stop(state);
    //            GameState.AddScript(new TutorialScript02());
    //        }

    //        _currentTime += time;

    //        base.Update(state, gameTime);
    //    }
    //}

    //public class TutorialScript02: TutorialScript
    //{
    //    private float _currentTime;
    //    private float _totalTime = 3500;

    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);

    //        GameState.Camera.FocusedAt = GameState.Level.Texts[1];
    //    }

    //    public override void Update( GameState state, GameTime time)
    //    {
    //        if (_currentTime >= _totalTime)
    //        {
    //            Stop(state);
    //            GameState.AddScript(new TutorialScript03());
    //        }

    //        _currentTime += (float) time.ElapsedGameTime.TotalMilliseconds;
            
    //        base.Update(state, time);
    //    }
    //}

    //public class TutorialScript03: TutorialScript
    //{
    //    private float _currentTime;
    //    private float _totalTime = 4000;

    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);

    //        GameState.IsHeartbeatEnabled = true;
    //        GameState.Camera.FocusedAt = GameState.Level.Texts[2];
    //    }

    //    public override void Update( GameState state, GameTime time)
    //    {
    //        if (_currentTime >= _totalTime)
    //        {
    //            Stop(state);
    //            GameState.AddScript(new TutorialScript04());
    //        }

    //        _currentTime += (float)time.ElapsedGameTime.TotalMilliseconds;

    //        base.Update(state, time);
    //    }
    //}

    //public class TutorialScript04: TutorialScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);

    //        var offset = new Vector2(540, 0);

    //        GameState.AreControlsEnabled = true;
    //        GameState.Player.WorldPosition = GameState.Level.Texts[3].WorldPosition - offset;
    //        GameState.Camera.FocusedAt = GameState.Player;
    //        GameState.Camera.FocusedAtOffset = new Vector2(370, 70) / GameState.Camera.Scale;
    //        Stop(state);
    //    }
    //}

    //public class TutorialScript05: TutorialScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);
    //        //GameState.Inventory.TryAdd(TestData.BuildMedkit(GameState.));
    //        GameState.IsInventoryEnabled = true;
    //        Stop(state);
    //    }
    //}

    //public class TutorialScript06: TutorialScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);

    //        GameState.IsHealthBarEnabled = true;
    //        GameState.IsGodModeEnabled = true;
    //        Stop(state);
    //    }
    //}

    //public class TutorialScript07: TutorialScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        //base.OnStart(state);

    //        //GameState.Inventory.Slots[1].ItemsCount = 10;
    //    }
    //}

    //public class TutorialScript08: TutorialScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        base.OnStart(state);

    //        var minHealth = 0.3f * GameState.Player.MaxHealth;
    //        if (GameState.Player.Health < minHealth)
    //        {
    //            GameState.Player.Health = minHealth;
    //        }
    //        GameState.IsGodModeEnabled = false;
    //    }
    //}

    //public class SkipTutorialScript: GameEntityScript
    //{
    //    protected override void OnStart(GameState state)
    //    {
    //        //base.OnStart(state);

    //        //state.Inventory.Slots[1].ItemsCount = 10;

    //        //state.Camera.Position = state.Player.WorldPosition;
    //        //state.Camera.FocusedAt = state.Player;
    //        //state.Camera.FocusedAtOffset = new Vector2(370, 70) / state.Camera.Scale;
    //        //state.IsHealthBarEnabled = true;
    //        //state.IsInventoryEnabled = true;
    //        //state.IsHeartbeatEnabled = true;
    //        //state.IsGodModeEnabled = false;
    //        //state.AreControlsEnabled = true;
    //        //Stop(state);
    //    }
    //}
}
