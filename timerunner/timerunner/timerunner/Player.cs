using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace timerunner
{
    class Player : Sprite
    {
        const string PLAYER_ASSETNAME = "Abaddon";
        const int PLAYER_SPEED = 160;
        const int MAX_JUMP_HEIGHT = 150;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        //const int MOVE_LEFT = -1;
        //const int MOVE_RIGHT = 1;
        const int BOT_DIST = 50;
        Vector2 mStartingPosition = Vector2.Zero;

        enum State
        {
            Walking,
            Jumping
        }
        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        public Player(int WindowWidth, int WindowHeight)
        {

            Position = new Vector2(WindowWidth, WindowHeight);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, PLAYER_ASSETNAME);
            Position = new Vector2((Position.X - (mSpriteTexture.Width)) / 2, Position.Y - mSpriteTexture.Height - BOT_DIST);
                  
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            UpdateJump(aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }

            if (mCurrentState == State.Jumping)
            {
                if (mStartingPosition.Y - Position.Y > MAX_JUMP_HEIGHT)
                {
                    mDirection.Y = MOVE_DOWN;
                }

                if (Position.Y > mStartingPosition.Y)
                {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                }
            }
        }

        private void Jump()
        {
            if (mCurrentState != State.Jumping)
            {
                mCurrentState = State.Jumping;
                mStartingPosition = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(PLAYER_SPEED, PLAYER_SPEED);
            }
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                //if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                //{
                //    mSpeed.X = PLAYER_SPEED;
                //    mDirection.X = MOVE_LEFT;
                //}
                //else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                //{
                //    mSpeed.X = PLAYER_SPEED;
                //    mDirection.X = MOVE_RIGHT;
                //}

                //if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                //{
                //    mSpeed.Y = PLAYER_SPEED;
                //    mDirection.Y = MOVE_UP;
                //}
                //else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                //{
                //    mSpeed.Y = PLAYER_SPEED;
                //    mDirection.Y = MOVE_DOWN;
                //}
            }
        }
    }
}
