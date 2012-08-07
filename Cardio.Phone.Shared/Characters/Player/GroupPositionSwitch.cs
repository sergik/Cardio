using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Characters.Player
{
    public class GroupPositionSwitch
    {
        private static Random Random = new Random();

        private IList<Nanobot> _bots;

        private IList<Vector2> _targetPositions;
        private IList<Vector2> _startPositions;

        public bool IsSwitching { get; private set; }

        private float _restTime = 16000;
        private float _currentRestTime;

        private float _switchTime = 1000;
        private float _currentSwitchTime;

        public GroupPositionSwitch(IList<Nanobot> bots)
        {
            _bots = bots;
            _startPositions = bots.Select(x => x.GroupPosition).ToList();
            _targetPositions = new List<Vector2>();
            for (int i = 0; i < _bots.Count; i++)
            {
                _targetPositions.Add(Vector2.Zero);
            }

            UpdateTargets();
        }

        public void Update(GameTime gameTime)
        {
            var time = (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!IsSwitching)
            {
                if (_currentRestTime >= _restTime)
                {
                    UpdateTargets();

                    _currentSwitchTime = 0;
                    IsSwitching = true;
                    _currentRestTime = 0;
                }
                else
                {
                    _currentRestTime += time;
                }
            }
            else
            {
                if (_currentSwitchTime <= _switchTime)
                {
                    // continue switch
                    var nextNormalizedTime = EaseMovement(MathHelper.Clamp((_currentSwitchTime + time) / _switchTime, 0f, 1f));
                    for (int i = 0; i < _bots.Count; i++)
                    {
                        var bot = _bots[i];
                        var distance = _targetPositions[i] - _startPositions[i];
                        bot.GroupPosition = _startPositions[i] + distance * nextNormalizedTime;
                    }

                    _currentSwitchTime += time;
                }
                else
                {
                    IsSwitching = false;
                    _currentSwitchTime = 0;
                    _currentRestTime = 0;
                }
            }
        }

        private float EaseMovement(float normalizedTime)
        {
            return (normalizedTime < 0.5f)
                ? EaseInExponential(normalizedTime * 2f) / 2f
                : ((1f - EaseInExponential((1f - normalizedTime) * 2f)) / 2f) + 0.5f;
        }

        private float EaseInExponential(float normalizedTime)
        {
            var a = 4f;
            return (float)((Math.Exp(a * normalizedTime) - 1.0) / (Math.Exp(a) - 1.0));
        }

        private void UpdateTargets()
        {
            for (int i = 0; i < _bots.Count; i++)
            {
                _startPositions[i] = _bots[i].GroupPosition;
            }

            var offset = Random.Next(1, 3);
            for (int i = 0; i < _bots.Count; i++)
            {
                _targetPositions[i] = _bots[(i + offset) % _bots.Count].GroupPosition;
            }
        }
    }
}
