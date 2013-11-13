using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace timerunner
{
    class Player : Sprite
    {
        // Constants
        const string PLAYER_ASSETNAME = "knight";
        const float PLAYER_SPEED = 300; // Walking Speed
        const double fireballEnergyIncrease = .003;
        
        const float MOVE_UP = -1.9f; // Speed when jumping up
        const float MOVE_DOWN = 1.7f;  // Speed when coming down after jump

        public double fireballEnergyPercentage = 0;
        

        bool SwordAttack = false;
        bool Injured = false;

        float BOT_DIST = 0; //Initialising distance from the ground, set again in the constructor
        float MAX_JUMP_HEIGHT = 0; // is set again in the constructor
        Vector2 mStartingPosition = Vector2.Zero;
        public List<Fireball> mFireballs = new List<Fireball>();
        ContentManager mContentManager;

        //Sound
        SoundEffect shootSound;

        enum State
        {
            Walking,
            Jumping,
            Falling
        }
        
        //Initialize Player Entity
        State mCurrentState = State.Falling;
        Vector2 mDirection = new Vector2(0, -1);
        Vector2 mSpeed = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;

        public Player(int WindowWidth, int WindowHeight, float jumpheight, float start)
        {
            Position = new Vector2(WindowWidth, WindowHeight);
            MAX_JUMP_HEIGHT = jumpheight;
            BOT_DIST = start;
        }

        // Override Draw Function to draw fireballs
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

            // Set starting position for Player 
            Position = new Vector2(((Position.X/2) - (texture.Width)-40) / 2, Position.Y - texture.Height - BOT_DIST);

            //Load sound effect
            shootSound = theContentManager.Load<SoundEffect>("Effect");
        }

        public void Update(GameTime theGameTime, bool intersects)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            mSpeed = new Vector2(PLAYER_SPEED, PLAYER_SPEED);
            UpdateFalling(intersects);
            UpdateJump(aCurrentKeyboardState);
            UpdateFireball(theGameTime, aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);

            //Controls how many fireballs you can shoot
            if (fireballEnergyPercentage < 1)
            {
                fireballEnergyPercentage += fireballEnergyIncrease;
            }
        }

        private void UpdateFalling(bool intersects)
        {
            if (mCurrentState == State.Falling)
            {
                if (intersects)
                {
                    mCurrentState = State.Walking;
                    mDirection = Vector2.Zero;
                }
                else
                {
                    mDirection.Y = MOVE_DOWN;
                }

            }

        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true && mPreviousKeyboardState.IsKeyDown(Keys.Up) == false)
                {
                    Jump();
                }
                else
                {
                    mCurrentState = State.Falling;
                }
            }

            if (mCurrentState == State.Jumping)
            {
                if (mStartingPosition.Y - Position.Y > MAX_JUMP_HEIGHT)
                {
                    mCurrentState = State.Falling;
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
                if (fireballEnergyPercentage > .333)
                {
                    ShootFireball();
                    shootSound.Play();
                    fireballEnergyPercentage -= .333;
                }
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

    }
}
