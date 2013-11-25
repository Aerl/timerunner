using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace timerunner
{
    public class Runner : AnimatedSprite
    {
        public enum State
        {
            Walking,Jumping,Falling,Die
        }
        public State currentState = State.Falling;

        public Runner()
            : base()
        {
            Animations.Add("walking", new Animation(new Vector2(10, 10), 90, 140, 0, 0, 7));
            Position = new Vector2(100, 100);
            CurrentAnimation = "walking";
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("RunSprites");
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 50.0f;
            Animating = false;

            if (kState.IsKeyDown(Keys.Q))
            {
                CurrentAnimation = "walking";
                Animating = true;
            }
            else
            {
                CurrentAnimation = "walking";
                Animating = true;
            }

            //if()

            HandleState();

            base.Update(gameTime);
        }

        private void HandleState()
        {
            if (currentState == State.Falling)  //falling
            {
                if (false) // on top of the platform
                {
                    currentState = State.Walking;
                }
                else
                {
                    Position.Y -= 10;
                }
            }

            //if (currentState == State.Jumping)  //jumping
            //{
            //    if (Position.Y - Position.Y > MAX_JUMP_HEIGHT)
            //    {
            //        mCurrentState = State.Falling;
            //    }

            //    if (Position.Y > mStartingPosition.Y)
            //    {
            //        Position.Y = mStartingPosition.Y;
            //        mCurrentState = State.Walking;
            //        mDirection = Vector2.Zero;
            //    }
            //}
        }
    }
}
