﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Cardio.Phone.Shared.Input
{
    public static class InputManager
    {
        #region Touch

        public static List<GestureSample> Gestures { get; set; }

        #endregion

        #region Mouse

        public static MouseState CurrentMouseState { get; private set; }

        private static MouseState _previousMouseState;

        public static bool IsMouseButtonTriggered(Func<MouseState, ButtonState> buttonSelector)
        {
            return buttonSelector(CurrentMouseState) == ButtonState.Pressed &&
                buttonSelector(_previousMouseState) == ButtonState.Released;
        } 

        public static bool IsMouseMoved()
        {
            return (_previousMouseState.X - CurrentMouseState.X + _previousMouseState.Y - CurrentMouseState.Y != 0);
        }

        public static float GetMouseWheelScroll()
        {
            return CurrentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }

        public static bool IsMouseUp()
        {
            return CurrentMouseState.LeftButton == ButtonState.Released &&
                   _previousMouseState.LeftButton == ButtonState.Pressed;
        }

        #endregion

        #region Keyboard

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState { get; private set; }


        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState _previousKeyboardState;


        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }


        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key)) &&
                (!_previousKeyboardState.IsKeyDown(key));
        }

        #endregion

#region Gamepad

        #endregion

        static InputManager()
        {
            Gestures = new List<GestureSample>();
        }

        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.VerticalDrag | GestureType.HorizontalDrag
                | GestureType.Flick | GestureType.DoubleTap | GestureType.Pinch;
            
            Gestures = new List<GestureSample>();
        }

        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            Gestures.Clear();

            // update the keyboard state
            _previousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            _previousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                Gestures.Add(gesture);
            }
        }

        public static bool IsExitPressed()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
        }

        public static bool IsButtonClicked(Rectangle position)
        {
            return IsMouseButtonTriggered(x => x.LeftButton) && position.Contains(CurrentMouseState.X, CurrentMouseState.Y);
        }

        public static bool IsTouched(Rectangle rectangle)
        {
            for (int i = 0; i < Gestures.Count; i++)
            {
                if (rectangle.Contains((int)Gestures[i].Position.X, (int)Gestures[i].Position.Y) || rectangle.Contains((int)Gestures[i].Position2.X, (int)Gestures[i].Position2.Y))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
