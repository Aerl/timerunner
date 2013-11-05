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
        const int PLAYER_SPEED = 300;
        const int MAX_JUMP_HEIGHT = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        //const int MOVE_LEFT = -1;
        //const int MOVE_RIGHT = 1;
        const int BOT_DIST = 50;
        Vector2 mStartingPosition = Vector2.Zero;
        List<Fireball> mFireballs = new List<Fireball>();
        ContentManager mContentManager;

        enum State
        {
            Walking,
            Jumping,
            Melee,
            Firing
        }
        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        public Player(int WindowWidth, int WindowHeight)
        {

            Position = new Vector2(WindowWidth, WindowHeight);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Draw(theSpriteBatch);
            }
            base.Draw(theSpriteBatch);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.LoadContent(theContentManager);
            }
            base.LoadContent(theContentManager, PLAYER_ASSETNAME);
            Position = new Vector2((Position.X - (mSpriteTexture.Width)) / 2, Position.Y - mSpriteTexture.Height - BOT_DIST);
                  
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            UpdateJump(aCurrentKeyboardState);
            UpdateFireball(theGameTime, aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true && mPreviousKeyboardState.IsKeyDown(Keys.Up) == false)
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

        private void UpdateFireball(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                ShootFireball();
            }
        }

        private void ShootFireball()
        {
            
                bool aCreateNew = true;
                foreach (Fireball aFireball in mFireballs)
                {
                    if (aFireball.Visible == false)
                    {
                        aCreateNew = false;
                        aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                            new Vector2(200, 0), new Vector2(1, 0));
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Fireball aFireball = new Fireball();
                    aFireball.LoadContent(mContentManager);
                    aFireball.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                        new Vector2(200, 200), new Vector2(1, 0));
                    mFireballs.Add(aFireball);
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
