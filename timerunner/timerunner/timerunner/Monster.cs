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
    class Monster : Sprite
    {

        string monster_AssetName = "walrus";
        const int MAX_JUMP_HEIGHT = 0;
        const float MOVE_UP = -1.7f;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const float MOVE_DOWN = 1.7f;
        int health = 1;
        int dyingHeight;
        ContentManager content;

        const int BOT_DIST = 50;
        Vector2 mStartingPosition = new Vector2(1000, -300);

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        public Monster(int speed)
        {
            Position = mStartingPosition;
            mSpeed.X = speed*2;
            mSpeed.Y = speed*2;
            dyingHeight = 30;
        }

        public enum State
        {
            Walking,
            Flying,
            Falling,
            Dead
        }

        public State mCurrentState = State.Falling;

        public void LoadContent(ContentManager theContentManager)
        {
            content = theContentManager;
            base.LoadContent(theContentManager, monster_AssetName);
            Position = mStartingPosition;
        }

        public void Update(GameTime theGameTime, bool intersects)
        {
            UpdateMovement();
            UpdateFalling(intersects);
            Die();

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement()
        {
            if (mCurrentState == State.Walking)
            {
                mDirection.X = MOVE_LEFT;
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

        private void Die()
        {
            if (mCurrentState == State.Dead)
            {
                if (dyingHeight > 0)
                {
                    dyingHeight--;
                    mDirection.Y = MOVE_UP;
                }
                else
                {
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }

        public void Hit()
        {
             health--;
             if (health <= 0)
             {
                 mCurrentState = State.Dead;
                 texture = content.Load<Texture2D>("deadWalrus"); 
             }  
        }
    }
}
