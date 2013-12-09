using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace timerunner
{
    class InputManager
    {
        // Current input states, to see what controls the user is actively pressing
        static public KeyboardState currentKeyboardState;
        static public Dictionary<PlayerIndex, GamePadState> currentGamePadState = new Dictionary<PlayerIndex,GamePadState>();

        // Input states from the last frame, to see what controls the user was pressing
        // (to compare with current & see if the user is holding a button down, releasing a button, etc)
        static public KeyboardState previousKeyboardState; 
        static public Dictionary<PlayerIndex, GamePadState> previousGamePadState = new Dictionary<PlayerIndex,GamePadState>();



        public InputManager()
        {
            // Prepare to get input from any controller port (Xbox allows 1-4)
            if (currentGamePadState.Count == 0)
            {
                currentGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                currentGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                currentGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                currentGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

                previousGamePadState.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
                previousGamePadState.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
                previousGamePadState.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
                previousGamePadState.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));
            }
        }

        public void SetCurrentInputState()
        {
            currentGamePadState[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            currentGamePadState[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            currentGamePadState[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            currentGamePadState[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            currentKeyboardState = Keyboard.GetState(PlayerIndex.One);
        }

        public void SetAsPreviousInputState()
        {
            previousGamePadState[PlayerIndex.One] = currentGamePadState[PlayerIndex.One];
            previousGamePadState[PlayerIndex.Two] = currentGamePadState[PlayerIndex.Two];
            previousGamePadState[PlayerIndex.Three] = currentGamePadState[PlayerIndex.Three];
            previousGamePadState[PlayerIndex.Four] = currentGamePadState[PlayerIndex.Four];

            previousKeyboardState = currentKeyboardState;
        }

        public bool IsPressed(Keys key, Buttons button)
        {
            // Checks keyboard for input of a specific key
            if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key) == false)
            {
                return true;
            }
            // Checks controller for input of a specific key
            else if (currentGamePadState[PlayerIndex.One].IsButtonDown(button) && previousGamePadState[PlayerIndex.One].IsButtonDown(button) == false)
            {
                return true;
            }
            return false;
        }


        /*
         * These more specific methods can go in Runner.cs:
         * */
        public bool PressedJump()
        {
            return IsPressed(Keys.Up, Buttons.A);
        }

        public bool PressedFireball()
        {
            return IsPressed(Keys.Space, Buttons.B);
        }

        public bool PressedMelee()
        {
            return IsPressed(Keys.A, Buttons.X);
        }
    }
}
