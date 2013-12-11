using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace timerunner
{
    class HelpClass
    {
        // Mouse coordinates are initialized at 0, 0
        public static Vector2 mousePosition = Vector2.Zero;
        bool IsMouseVisible = true;

        public static void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            // The mouse X and Y positions are returned relative to the top-left of the game window.
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;
        }

        bool MouseOverSprite = false;
        void OnMouseOver(Vector2 spritePosition, Texture2D spriteTexture)
        {
            if ((mousePosition.X >= spritePosition.X) && mousePosition.X <= (spritePosition.X + spriteTexture.Width) &&
                    mousePosition.Y >= spritePosition.Y && mousePosition.Y <= (spritePosition.Y + spriteTexture.Height))
                MouseOverSprite = true;
            else MouseOverSprite = false;
        } // end OnMouseOver


        bool PixelDetected = false;
        Vector2 pixelPosition = Vector2.Zero;
        uint[] PixelData = new uint[1]; // Delare an Array of 1 just to store data for one pixel
        void PixelCheck(Vector2 spritePosition, Texture2D spriteTexture)
        {
            // Get Mouse position relative to top left of Texture
            pixelPosition = mousePosition - spritePosition;

            // I know. I just checked this condition at OnMouseOver or we wouldn't be here
            // but just to be sure the spot we're checking is within the bounds of the texture...
            if (pixelPosition.X >= 0 && pixelPosition.X < spriteTexture.Width &&
                pixelPosition.Y >= 0 && pixelPosition.Y < spriteTexture.Height)
            {
                // Get the Texture Data within the Rectangle coords, in this case a 1 X 1 rectangle
                // Store the data in pixelData Array
                spriteTexture.GetData<uint>(0, new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, (1), (1)), PixelData, 0, 1);

                // Check if pixel in Array is non Alpha, give or take 20
                if (((PixelData[0] & 0xFF000000) >> 24) > 20)
                    PixelDetected = true;
                else PixelDetected = false;
            }
        } // end PixelCheck

        //this is not pixelcheck
        public static bool checkMouseClickOnSprite(Vector2 spritePosition, Texture2D spriteTexture)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if ((mousePosition.X >= spritePosition.X) && mousePosition.X <= (spritePosition.X + spriteTexture.Width) &&
                        mousePosition.Y >= spritePosition.Y && mousePosition.Y <= (spritePosition.Y + spriteTexture.Height))
                    return true;
                else
                    return false;
            }
            else
                return false;
        } // end checkMouseClick

        #region other helper class
        public static bool IntersectPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color c1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color c2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (c1.A != 0 && c2.A != 0)
                        return true;
                }

            return false;
        }

        public static bool IntersectRunner(Runner r1, Sprite r2)
        {
            int x = (int)r1.Position.X - 40;
            int y = (int)r1.Position.Y - 60;
            Rectangle runnerRectangle = new Rectangle(x, y, r1.Animations[r1.CurrentAnimation].Width, r1.Animations[r1.CurrentAnimation].Height);


            int row = 0;
            int column = 0;

            if (r1.CurrentAnimation == "walking")
                row = 0;
            else if (r1.CurrentAnimation == "melee")
                row = 1;
            else if (r1.CurrentAnimation == "fire")
                row = 2;
            else if (r1.CurrentAnimation == "jump")
                row = 3;

            column = r1.currentFrame;
            Texture2D rectangleTexture = new Texture2D(Game1.Instance.GraphicsDevice, r1.Animations[r1.CurrentAnimation].Width, r1.Animations[r1.CurrentAnimation].Height, true, SurfaceFormat.Color);

            int arrayLength = r1.Animations[r1.CurrentAnimation].Width * r1.Animations[r1.CurrentAnimation].Height;

            Color[] fullTextureData;
            Color[] specificTextureData = new Color[arrayLength];

            fullTextureData = new Color[r1.Sprite.Width * r1.Sprite.Height];
            r1.Sprite.GetData(fullTextureData);

            for (int i = 0; i < r1.Animations[r1.CurrentAnimation].Height; i++)
            {
                for (int j = 0; j < r1.Animations[r1.CurrentAnimation].Width; j++)
                {
                    int a = 825 * i + j + row * 825 * 120 + column * 101;
                    int a1 = 101 * i + j;
                    if (a1 == 101)
                    {
                        string walrus = "";
                    }
                    specificTextureData[a1] = fullTextureData[a];
                }
            }
            
            return IntersectPixel(runnerRectangle,specificTextureData,r2.Size,r2.textureData);
        }

        public static bool IsRunnerOnTopOf(Runner r1, Platform platform)
        {
            Rectangle runnerRectangle = new Rectangle((int)r1.Position.X, (int)r1.Position.Y - 75, r1.Animations[r1.CurrentAnimation].Width, r1.Animations[r1.CurrentAnimation].Height);

            return platform.Size.Top+10 >= runnerRectangle.Bottom && platform.Size.Top-10<= runnerRectangle.Bottom && runnerRectangle.Left >= platform.Size.Left && runnerRectangle.Right - runnerRectangle.Width <= platform.Size.Right;
        }

        public static bool IsMonsterInBounds(Sprite monster, Platform platform)
        {
            Rectangle monsterRectangle = monster.Size;
            return platform.Size.Top + 30 >= monsterRectangle.Bottom && platform.Size.Top - 30 <= monsterRectangle.Bottom && monsterRectangle.Left >= platform.Size.Left-50 && monsterRectangle.Right - monsterRectangle.Width <= platform.Size.Right;
        }

        public static void CreateRunnerRectangle(Runner r1)
        {
            int x = (int)r1.Position.X - 50;
            int y = (int)r1.Position.Y - 60;

            int row = 0;
            int column = 0;

            if (r1.CurrentAnimation == "walking")
                row = 0;
            else if (r1.CurrentAnimation == "swordAttack")
                row = 1;
            else if (r1.CurrentAnimation == "fire")
                row = 2;
            else if (r1.CurrentAnimation == "jump")
                row = 3;

            column = r1.currentFrame;
            Texture2D rectangleTexture = new Texture2D(Game1.Instance.GraphicsDevice, r1.Animations[r1.CurrentAnimation].Width, r1.Animations[r1.CurrentAnimation].Height, true, SurfaceFormat.Color);

            int arrayLength = r1.Animations[r1.CurrentAnimation].Width * r1.Animations[r1.CurrentAnimation].Height;

            Color[] fullTextureData;
            Color[] specificTextureData = new Color[arrayLength];
            Color[] halfTextureData;

            fullTextureData = new Color[r1.Sprite.Width * r1.Sprite.Height];
            r1.Sprite.GetData(fullTextureData);

            for (int i = 0; i < r1.Animations[r1.CurrentAnimation].Height; i++)
            {
                for (int j = 0; j < r1.Animations[r1.CurrentAnimation].Width; j++)
                {
                    int a = 825 * i + j + row * 825 * 120 + column * 101;
                    int a1 = 101 * i + j;
                    if (a1 == 101)
                    {
                        string walrus = "";
                    }
                    specificTextureData[a1] = fullTextureData[a];
                }
            }
            rectangleTexture.SetData(specificTextureData);
            Game1.Instance.spriteBatch.Draw(rectangleTexture, new Vector2(x, y), Color.White);
        }

        //We need to figure out changeInX and changeInY
        public static Vector2 GenerateRandomLandLocation(int max, float yPreviousLocation, int changeInX, int changeInY)
        {
             double slope = (double)changeInY * -1 / (double)changeInX;
            int yPrev = Convert.ToInt32(yPreviousLocation);
            int randomY = 0;
            int randomX = 0;
            int ADJUST_X = 60 + changeInX/20;
            int maxJumpHeight = max - 30;
            while (randomX < ADJUST_X)
            {
                Random r = new Random();
                if (yPrev - maxJumpHeight < 100)
                    randomY = r.Next(100, 600);
                else
                    randomY = r.Next(yPrev - maxJumpHeight, 600);
                int x1 = Convert.ToInt32((randomY - yPrev) / slope);
                int x2 = Convert.ToInt32((randomY - yPrev + 2 * maxJumpHeight) / (-1 * slope));

                if (changeInX > changeInY)
                {
                    string walrus = "walrus";
                }

                if (x2 >  ADJUST_X + changeInX / 10)
                {
                    if (randomY > yPrev)
                    {
                        randomX = r.Next(ADJUST_X + changeInX / 10, x2);
                    }
                    else
                    {
                        randomX = r.Next(x1, x2);
                    }
                }
            }
            return new Vector2(randomX + 1000, randomY);
        }
        #endregion
    }
}
