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
        int monster_Speed = 40;
        const int MAX_JUMP_HEIGHT = 0;
        //const int MOVE_UP = -1;
        //const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        const int BOT_DIST = 50;
        Vector2 mStartingPosition = new Vector2(800,300);

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        public Monster(int WindowWidth, int WindowHeight)
        {
            //Position = new Vector2(WindowWidth, WindowHeight);
        }

        enum State
        {
            Walking,
            Flying
        }

        State mCurrentState = State.Walking;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, monster_AssetName);
            Position = mStartingPosition;
        }

        public void Update(GameTime theGameTime)
        {
            UpdateMovement();

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement()
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                mSpeed.X = monster_Speed;
                mDirection.X = MOVE_LEFT;
            }
        }

    }
}
