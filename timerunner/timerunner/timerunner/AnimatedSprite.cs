using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace timerunner
{
    public class AnimatedSprite : GameEntity
    {
        float timer = 0f;

        public string assetName = "";

        public Rectangle Size = new Rectangle();

        public Color[] textureData;

        public Texture2D texture;

        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }
        float interval = 0.2f;

        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }
        int currentFrame = 0;

        string currentAnimation;
        bool animating;

        public bool Animating
        {
            get { return animating; }
            set { animating = value; }
        }

        public string CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }
        Rectangle sourceRect;
        Vector2 centre;

        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        public Dictionary<string, Animation> Animations
        {
            get { return animations; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public AnimatedSprite()
        {
            this.animating = false;
        }

        KeyboardState currentKBState;
        KeyboardState previousKBState;

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>(assetName);

        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Sprite, Position, SourceRect, Color.White, 0f, centre, 1.0f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {

            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            Animation animation = animations[currentAnimation];

            // Was I previously doing another animation
            if ((currentFrame < animation.StartFrame) || (currentFrame > animation.EndFrame))
            {
                currentFrame = animation.StartFrame;
            }

            sourceRect = new Rectangle((int)animation.Origin.X + (currentFrame * (animation.Width + animation.SpacingX))
                , (int)animation.Origin.Y
                , animation.Width
                , animation.Height);
            
            centre = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
           
            Size = new Rectangle((int)Position.X
            , (int)Position.Y
            , animation.Width
            , animation.Height);

            //textureData = new Color[Sprite.Width * Sprite.Height];
            //Sprite.GetData<Color>(0, sourceRect, textureData, sourceRect.X * sourceRect.Y, sourceRect.Width * sourceRect.Height);

            //textureData = new Color[Size.Width * Size.Height];
            //Sprite.GetData<Color>(0, sourceRect, textureData, sourceRect.X * sourceRect.Y, sourceRect.Width * sourceRect.Height);

            centre = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!animating)
            {
                currentFrame = animation.StartFrame;
            }
            else
            {
                if (timer > interval)
                {
                    currentFrame++;

                    if (currentFrame > animation.EndFrame)
                    {
                        currentFrame = animation.StartFrame;
                    }
                    timer = 0f;
                }
            }
        }
    }
}
