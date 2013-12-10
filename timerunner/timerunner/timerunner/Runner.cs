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
        public float walkingInterval = .2f;
        public float meleeInterval = .06f;
        public int widthOfSprite = 101;
        public int heightOfSprite = 120;

        InputManager im = new InputManager();

        public bool intersects;

        public float startJumpPosition = 0;

        //Score
        public float score = 0;
        
        //fireball
        public double fireballEnergyIncrease = .004;
        public double fireballEnergyPercentage = 0;
        public List<Fireball> mFireballs = new List<Fireball>();
        private Texture2D Fire;

        //Melee
        public bool SwordAttack = false;
        const int MELEE_TIME = 15; // time attack lasts
       
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
            Animations.Add("walking", new Animation(new Vector2(0, 0), widthOfSprite, heightOfSprite, 0, 0, 7));
            Animations.Add("fire", new Animation(new Vector2(0, heightOfSprite*2), widthOfSprite, heightOfSprite, 0, 0, 0));
            Animations.Add("swordAttack", new Animation(new Vector2(0, heightOfSprite), widthOfSprite, heightOfSprite, 0, 0, 4));
            Animations.Add("jump", new Animation(new Vector2(0, heightOfSprite*3), widthOfSprite, heightOfSprite, 0, 0, 0));
            Position = new Vector2(120, 170);
            CurrentAnimation = "walking";
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("KnightSprites");
            Fire = Game1.Instance.Content.Load<Texture2D>("Fireball");
            shootSound = Game1.Instance.Content.Load<SoundEffect>("Effect");
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) 
        {
            score++;
            im.SetCurrentInputState();
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Animating = true;

            if (SwordAttack)
            {
                Interval = meleeInterval;
                CurrentAnimation = "swordAttack";
            }
            else
            {
                Interval = walkingInterval;
                CurrentAnimation = "walking";
            }

            UpdateFireball(gameTime);
            UpdateState(intersects);
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
            im.SetAsPreviousInputState();
            
        }

        private void UpdateState(bool onPlatform)
        {
            if (currentState == State.Falling)
            {
                if (onPlatform)
                {
                    currentState = State.Walking;
                }
                else
                {
                    if (!SwordAttack)
                    CurrentAnimation = "jump";
                    Position.Y += MOVE_DOWN;
                }
            }
 
            if (currentState == State.Walking)
            {
                if (PressedJump())
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
                if (!SwordAttack)
                CurrentAnimation = "jump";
                if (startJumpPosition - Position.Y > MAX_JUMP_HEIGHT)
                {
                    currentState = State.Falling;
                }
            }

            if (SwordAttack)
            {
                if (currentFrame == 4)
                {
                    SwordAttack = false;
                }  
            }
            else
            {
                if (PressedMelee())
                {
                    if (fireballEnergyPercentage > .6)
                    {
                        fireballEnergyPercentage -= .6;
                        currentFrame = 0;
                        SwordAttack = true;
                    }
                }
            }
        }

        private void UpdateFireball(GameTime theGameTime)
        {
            foreach (Fireball aFireball in mFireballs)
            {
                aFireball.Update(theGameTime);
            }

            if (PressedFireball())
            {
                if (fireballEnergyPercentage > .3)
                {
                    ShootFireball();
                    //CurrentAnimation = "fire";
                    shootSound.Play();
                    fireballEnergyPercentage -= .3;
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
                CurrentAnimation = "fire";
                Fireball aFireball = new Fireball();
                aFireball.LoadContent(Fire, "Fireball");
                aFireball.Fire(Position + new Vector2(0,0),
                    new Vector2(1,1), new Vector2(2, 0));
                mFireballs.Add(aFireball);
            }
        }

        public bool PressedJump()
        {
            return im.IsPressed(Keys.Up, Buttons.A);
        }

        public bool PressedFireball()
        {
            return im.IsPressed(Keys.Space, Buttons.B);
        }

        public bool PressedMelee()
        {
            return im.IsPressed(Keys.A, Buttons.X);
        }
    }
}
