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
        public const float MOVE_UP = -14;
        public float MOVE_DOWN =14; 
        public int MAX_JUMP_HEIGHT = 300; 

        public bool intersects;

        public float startJumpPosition = 0;

        //Score
        public float score = 0;
        
        //fireball
        const double fireballEnergyIncrease = .004;
        public double fireballEnergyPercentage = 0;
        public List<Fireball> mFireballs = new List<Fireball>();
        private Texture2D Fire;
        KeyboardState PreviousKeyboardState;

        //Melee
        public bool SwordAttack = false;
        const int MELEE_TIME = 15; // time attack lasts
        int meleeCounter;
       
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
            Animations.Add("walking", new Animation(new Vector2(0, 0), 44, 80, 0, 0, 7));
            Position = new Vector2(120, 170);
            CurrentAnimation = "walking";
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("RunSprites1");
            Fire = Game1.Instance.Content.Load<Texture2D>("Fireball");
            shootSound = Game1.Instance.Content.Load<SoundEffect>("Effect");
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) 
        {
            score++;
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

            UpdateFireball(gameTime, kState);
            UpdateState(intersects, kState);
            base.Update(gameTime);

            //Controls how many fireballs you can shoot
            if (fireballEnergyPercentage < 1)
            {
                fireballEnergyPercentage += fireballEnergyIncrease;
            }

            //if the player is under the game screen, then die
            if (this.Position.Y > 700)
            {
                currentState = State.Die;
            }
            PreviousKeyboardState = kState;
            
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

            if (SwordAttack)
            {
                if (meleeCounter > MELEE_TIME)
                {
                    SwordAttack = false;
                }
                else
                {
                    meleeCounter++;
                }
            }
            else
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    SwordAttack = true;
                    meleeCounter = 0;
                }
            }
        }

        private void UpdateFireball(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) && PreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                if (fireballEnergyPercentage > .25)
                {
                    ShootFireball();
                    shootSound.Play();
                    fireballEnergyPercentage -= .25;
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
                    aFireball.Fire(Position + new Vector2(0,0),
                    new Vector2(1,1), new Vector2(2, 0));
                    break;
                }
            }

            if (aCreateNew == true)
            {
                Fireball aFireball = new Fireball();
                aFireball.LoadContent(Fire, "Fireball");
                aFireball.Fire(Position + new Vector2(0,0),
                    new Vector2(1,1), new Vector2(2, 0));
                mFireballs.Add(aFireball);
            }
        }
    }
}
