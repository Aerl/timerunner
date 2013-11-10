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
        const float PLATFORM_SPEED = 100; // Walking Speed
        Vector2 mSpeed = new Vector2(PLATFORM_SPEED, PLATFORM_SPEED);
        public Platform(String Asset, Vector2 newPosition)
        {
            //Constructor requires 2-Elementvector and texturename
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
