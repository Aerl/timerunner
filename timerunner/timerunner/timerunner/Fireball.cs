using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace timerunner
{
    class Fireball : Sprite
    {
        const int MAX_DISTANCE = 500;
        float scale = 0.3f;

        public bool Visible = false;

        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;

        String name = "Fireball";

        public void LoadContent(Texture2D tex, String theAssetName)
        {
            texture = tex;
            AssetName = theAssetName;
            Size = new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            //Creates an array of pixels
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            Scale = scale;

        }

        public void Update(GameTime theGameTime)
        {
            if (Vector2.Distance(mStartPosition, Position) > MAX_DISTANCE)
            {
                Visible = false;
                Position.X = 2000;
                Size.X = 2000;
            }

            if (Visible == true)
            {
                base.Update(theGameTime, mSpeed, mDirection);
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                base.Draw(theSpriteBatch);
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
            Visible = true;
        }
    }


}
