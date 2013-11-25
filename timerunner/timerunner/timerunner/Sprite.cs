using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace timerunner
{
    class Sprite
    {
        public int changeSpeed = 0;
        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1.0f;

        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        public Texture2D texture;

        public Color[] textureData;

        public Vector2 mSpeed = Vector2.Zero;

        //When the scale is modified throught he property, the Size of the 
        //sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
                
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            //Creates an array of pixels
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            ChangeSpeed();
            Position += theDirection * mSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            Size.X = Convert.ToInt32(Position.X);
            Size.Y = Convert.ToInt32(Position.Y);
        }

        public void ChangeSpeed()
        {
            if (mSpeed.X != Game1.gameSpeed)
            {
                mSpeed.X = Game1.gameSpeed;
                mSpeed.Y = 300;
            }
        }
    }
}
