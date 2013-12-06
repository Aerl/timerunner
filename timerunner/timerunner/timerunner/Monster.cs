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
    public class Monster : Sprite
    {
        //Constants and items that are similar throughout
        const float DIE_DOWN = 3;
        const float DIE_UP = -3;
        const float FALL = 3;
        ContentManager content;
        Vector2 mDirection = Vector2.Zero;   
        int dyingHeight;
        FlyingState flyingState = FlyingState.FlyingDown;

        //change in setup
        string monster_AssetName;
        string monster_DeadAssetName;
        int moveLeft;
        int moveRight;
        int moveUp;
        int moveDown;
        int health;
        public State mCurrentState;
        public bool intersects = false;

        Vector2 mStartingPosition = new Vector2(900, 0);

        public void Setup(int speed, string assetName, string deadAssetName, int moveLeft, int moveRight, int moveUp, int moveDown, int health, Vector2 startingPosition, State currentState)
        {
            monster_AssetName = assetName;
            monster_DeadAssetName = deadAssetName;
            this.moveLeft = moveLeft;
            this.moveRight = moveRight;
            this.moveUp = moveUp;
            this.moveDown = moveDown;
            this.health = health;
            mCurrentState = currentState;
            mStartingPosition = startingPosition;
            Position = mStartingPosition;

            dyingHeight = 20;
        }

        public enum State
        {
            Walking,
            Flying,
            Falling,
            Dead
        }

        public enum FlyingState
        {
            FlyingUp,
            FlyingDown
        }

        public void LoadContent(ContentManager theContentManager)
        {
            content = theContentManager;
            base.LoadContent(theContentManager, monster_AssetName);
            Position = mStartingPosition;
        }

        public void Update(GameTime theGameTime)
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
                mDirection.X = moveLeft;
            }

            if (mCurrentState == State.Flying)
            {
                mDirection.X = moveLeft;
                if (Position.Y < 0)
                {
                    mDirection.Y = moveDown;
                    flyingState = FlyingState.FlyingDown;
                }
                else if (Position.Y > 600)
                {
                    mDirection.Y = moveUp;
                    flyingState = FlyingState.FlyingUp;
                }
                else
                {
                    if (flyingState == FlyingState.FlyingDown)
                        mDirection.Y = moveDown;
                    else
                        mDirection.Y = moveUp;
                }
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
                    mDirection.Y = FALL;
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
                    mDirection.Y = DIE_UP;
                }
                else
                {
                    mDirection.Y = DIE_DOWN;
                }
            }
        }

        public void Hit(ref float score)
        {
             health--;
             if (health == 0)
             {
                 mCurrentState = State.Dead;
                 score += 1000;
                 texture = content.Load<Texture2D>(monster_DeadAssetName); 
             }  
        }

        public void HitByMelee(ref float score)
        {
            health = 1;
            Hit(ref score);
        }
    }
}
