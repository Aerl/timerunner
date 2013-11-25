using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace timerunner
{
    class Platform : Sprite
    {
        ContentManager mContentManager;
        string Platform_ASSETNAME = ""; // texture name is set in constructor
        Vector2 mDirection = new Vector2(-1, 0);
        Vector2 mSpeed;
        public Platform(String Asset, Vector2 newPosition, int gameSpeed)
        {
            //Constructor requires 2-Elementvector and texturename
            mSpeed = new Vector2(gameSpeed, gameSpeed);
            Platform_ASSETNAME = Asset;
            Position = newPosition;

        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            // Load texture
            base.LoadContent(theContentManager, Platform_ASSETNAME);

        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, mSpeed, mDirection);
        }
    }
}
