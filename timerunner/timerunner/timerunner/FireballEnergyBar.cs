using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace timerunner
{
    class FireballEnergyBar : Sprite
    {
        string name = "FireballEnergyBar";
        float scale = 0.3f;
        Vector2 mSpeed;
        Vector2 mDirection;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, name);
            Scale = scale;
            // Set  position for bar
            Position = new Vector2(20,100);

        }

        public void Update(GameTime theGameTime)
        {
             base.Update(theGameTime, mSpeed, mDirection);
        }

        public void Draw(SpriteBatch theSpriteBatch, GraphicsDeviceManager graphics, double fireballEnergyPercentage)
        {
            base.Draw(theSpriteBatch);
            //height between 102 and 278
            //269
            int yLocation = 101 + 1 + Convert.ToInt32(176 * (1-fireballEnergyPercentage));
            theSpriteBatch.Draw(CreateRectangle(graphics,fireballEnergyPercentage),new Vector2(22,yLocation), Color.White);
        }

        private Texture2D CreateRectangle(GraphicsDeviceManager graphics,double fireballEnergyPercentage)
        {
            //height between 0 and 176
            int height = 1 + Convert.ToInt32(176 * fireballEnergyPercentage);
            int width = 14;
            Texture2D rectangleTexture = new Texture2D(graphics.GraphicsDevice, width, height, true,SurfaceFormat.Color);// create the rectangle texture, ,but it will have no color! lets fix that
            Color[] color = new Color[width * height];//set the color to the amount of pixels in the textures
            for (int i = 0; i < color.Length; i++)//loop through all the colors setting them to whatever values we want
            {
                color[i] = Color.Red;
            }
            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;//return the texture
        }

    }


}
