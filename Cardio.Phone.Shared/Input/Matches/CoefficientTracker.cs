using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Input.Matches
{
    public class CoefficientTracker
    {
        private readonly TouchSequenceMatchStrategy _strategy;

        private readonly int _beatsCount;

        private readonly float _delta;

        private readonly Queue<float> _coefficientsHistory;

        public event EventHandler CoefficientProperlyChanged;

        public CoefficientTracker(int beatsCount, float delta, TouchSequenceMatchStrategy strategy)
        {
            _beatsCount = beatsCount;
            _delta = delta;
            _strategy = strategy;
            _coefficientsHistory = new Queue<float>(beatsCount);
        }

        public void Update()
        {
            RememberCoeff();

            if (_coefficientsHistory.Count == _beatsCount &&
                _strategy.MatchCoefficient - _coefficientsHistory.Peek() >= _delta)
            {
                System.Diagnostics.Debug.WriteLine("Called");
                _coefficientsHistory.Clear();
                CoefficientProperlyChanged.Fire(this, () => EventArgs.Empty);
            }
        }

        private void RememberCoeff()
        {
            _coefficientsHistory.Enqueue(_strategy.MatchCoefficient);
            if(_coefficientsHistory.Count > _beatsCount)
            {
                _coefficientsHistory.Dequeue();
            }
        }

    }
}
