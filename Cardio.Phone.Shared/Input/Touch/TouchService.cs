using System;
using System.Collections.Generic;

namespace Cardio.Phone.Shared.Input.Touch
{
    public class TouchService : ITouchService
    {
        private readonly List<ButtonTouch> _userTouches = new List<ButtonTouch>();

        public void Push(ButtonTouch touch)
        {
            _userTouches.Add(touch);
        }

        public void Reset()
        {
            _userTouches.Clear();
        }

        public List<ButtonTouch> Touches
        {
            get { return _userTouches; }
        }

        public bool ContainsValidSequence()
        {
            for (int i = 0; i < _userTouches.Count -1; i ++)
            {
                var currentTouch = _userTouches[i];
                if (currentTouch.TouchType == TouchType.NotAllowed)
                {
                    return false;
                }

                if (_userTouches[i].TouchNumber + 1 != _userTouches[i + 1].TouchNumber)
                {
                    return false;
                }
            }

            if (_userTouches.Count > 0)
            {
                return _userTouches[_userTouches.Count - 1].TouchType != TouchType.NotAllowed;
            }

            return true;
        }
    }
}