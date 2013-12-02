using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace timerunner
{
    class Runner : AnimatedSprite
    {
        //Variables
        public const float MOVE_UP = -15;
        public const float MOVE_DOWN =15; 
        public int MAX_JUMP_HEIGHT = 300; 

        public bool intersects;

        public float startJumpPosition = 0;

        //Score
        public float score = 0;
        
        //fireball
        const double fireballEnergyIncrease = .004;
        public double fireballEnergyPercentage = 0;
        public List<Fireball> mFireballs = new List<Fireball>();

        //Sound
        SoundEffect shootSound;

        public enum State
        {
            Walking,Jumping,Falling,Die
        }
        public State currentState = State.Falling;

        public Runner()
            : base()
        {
            Animations.Add("walking", new Animation(new Vector2(0, 0), 44, 68, 0, 0, 7));
            Position = new Vector2(120, 170);
            CurrentAnimation = "walking";
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("RunSprites1");
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) 
        {
            KeyboardState kState = Keyboard.GetState();
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 200.0f;
            Animating = true;

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

            UpdateState(intersects, kState);
            base.Update(gameTime);

            //if the player is under the game screen, then die
            if (this.Position.Y > 700)
            {
                currentState = State.Die;
            }
            
        }

        private void UpdateState(bool onPlatform, KeyboardState aCurrentKeyboardState)
        {
            if (currentState == State.Falling)
            {
                if (onPlatform)
                {
                    currentState = State.Walking;
                }
                else
                    Position.Y += MOVE_DOWN;
            }
 
            if (currentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    startJumpPosition = Position.Y;
                    currentState = State.Jumping;
                }
                if (!onPlatform)
                {
                    currentState = State.Falling;
                }
            }

            if (currentState == State.Jumping)
            {
                Position.Y += MOVE_UP;
                if (startJumpPosition - Position.Y > MAX_JUMP_HEIGHT)
                {
                    currentState = State.Falling;
                }
            }
        }
    }
}
