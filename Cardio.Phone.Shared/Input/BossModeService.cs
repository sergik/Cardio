using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Characters.Bosses;
using Cardio.UI.Core.Alive;
using Cardio.UI.Extensions;
using Cardio.UI.Core;
using Cardio.UI.Input.Matches;
using Cardio.UI.Rhythm;
using Cardio.UI.Rhythm.BeatGenerators;
using Cardio.UI.Scripts;

namespace Cardio.UI.Input
{
    public class BossModeService
    {
        private WindowsInputHandler _inputHandler;

        private List<TouchSequenceEntry> _combinationToKillBoss;

        private GameState _gameState;

        private RhythmEngine _rhytmEngine;

        private List<CoefficientTracker> _trackers = new List<CoefficientTracker>();

        private List<CoefficientTracker> _trackersToRemove = new List<CoefficientTracker>();

        private TouchSequenceMatchStrategy _strategy;

        public Boss Boss { get; set; }

        private ContinuousPlayerShootScript _continuousBossKillingScript;

        public BossModeService(WindowsInputHandler inputHandler, List<TouchSequenceEntry> combinationToKillBoss, GameState gameState)
        {
            _inputHandler = inputHandler;
            _gameState = gameState;
            _combinationToKillBoss = combinationToKillBoss;
            _rhytmEngine = _gameState.Game.Services.GetService<RhythmEngine>();
        }

        public void SwitchBossModeOn()
        {
            _rhytmEngine.PatternGenerator =
                new TimeSequenceBeatPatternGenerator(
                    _combinationToKillBoss.Select(entry => entry.MilisecondsBefore).ToList());

            _strategy = new TouchSequenceMatchStrategy(_gameState,
                                                             _combinationToKillBoss.Select(
                                                                 entry => entry.Button).ToList());

            _strategy.Matched += (o, e) => UpdateAfterBeat();
            _strategy.Failed += (o, e) => UpdateAfterBeat();

            //_trackers.Add(GetUseShieldsTracker());
            _trackers.Add(GetShootTracker());
            _gameState.Combo.StopSound();
            _inputHandler.MatchStrategy = _strategy;
        }

        private CoefficientTracker GetUseShieldsTracker()
        {
            var tracker = new CoefficientTracker(10, 0.01f, _strategy);
            tracker.CoefficientProperlyChanged += (o, e) => _gameState.Inventory.UseItemFromSlot(1, _gameState);
            return tracker;
        }

        private CoefficientTracker GetShootTracker()
        {
            Func<ContinuousPlayerShootScript> createShotScript = () => new ContinuousPlayerShootScript(new IAlive[] { Boss }) { ShootingTime = 2500 };

            const float shootAtBossTime = 2500;
            var tracker = new CoefficientTracker(5, 0.01f, _strategy);
            tracker.CoefficientProperlyChanged += (o, e) =>
                                                      {
                                                          if (!_gameState.Player.IsShooting)
                                                          {
                                                              _continuousBossKillingScript = createShotScript();
                                                              _gameState.AddScript(_continuousBossKillingScript);
                                                          }
                                                          else
                                                          {
                                                              _continuousBossKillingScript.ContinueFor(shootAtBossTime);
                                                          }
                                                      };
            return tracker;
        }

        public void SwithBossModeOff()
        {
            _rhytmEngine.PatternGenerator = new BeatPatternGenerator(500);
            var strategy = new DefaultMatchStrategy(_gameState);
            strategy.Failed += (o, e) =>
                                   {
                                       _gameState.Combo.Reset();
                                       _gameState.Player.Confuse();
                                   };
            _inputHandler.MatchStrategy = strategy;
        }

        private void UpdateAfterBeat()
        {
            if (_strategy.BeatController.CurrentBeatNumber == _combinationToKillBoss.Count)
            {
                _gameState.Player.Health = 0;
            }

            _trackers.ForEach(tracker => tracker.Update());
            _trackersToRemove.ForEach(trackerToRemove => _trackers.Remove(trackerToRemove));
        }
    }
}
