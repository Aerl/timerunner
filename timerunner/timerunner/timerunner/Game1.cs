using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace timerunner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Create a Horizontally scrolling background
        HorizontallyScrollingBackground mScrollingBackground;

        Player firstPlayerSprite;
        Monster monsterTrial;
        FireballEnergyBar fireballEnergyBar;
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;

        //Sound
        Song backgroundSong;

        // Player and Window Constants
        const float PLAYER_JUMP_HEIGHT = 300;
        const float PLAYER_INIT_HEIGHT = 500;
        const int WINDOW_HEIGHT = 700;
        const int WINDOW_WIDTH = 1000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize Player
            firstPlayerSprite = new Player(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, PLAYER_JUMP_HEIGHT, PLAYER_INIT_HEIGHT);

            // Initialize Monters
            monsterTrial = new Monster(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            fireballEnergyBar = new FireballEnergyBar();

            // Initialize Platforms
            platforms.Add(new Platform("Platform", new Vector2(30, 400)));
            platforms.Add(new Platform("Platform", new Vector2(350, 300)));
            platforms.Add(new Platform("Platform", new Vector2(700, 350)));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");

            // Load Playercontent
            firstPlayerSprite.LoadContent(this.Content);

            // Load Monstercontent
            monsterTrial.LoadContent(this.Content);

            fireballEnergyBar.LoadContent(this.Content);

            // TODO: Load any ResourceManagementMode.Automatic content


            mScrollingBackground = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);
            //mScrollingBackground.AddBackground("Background01");
            //mScrollingBackground.AddBackground("Background02");
            //mScrollingBackground.AddBackground("Background03");
            //mScrollingBackground.AddBackground("Background04");
            //mScrollingBackground.AddBackground("Background05");
            mScrollingBackground.AddBackground("sunrise");
            mScrollingBackground.AddBackground("daywclouds");
            mScrollingBackground.AddBackground("dusk");
            mScrollingBackground.AddBackground("nightwstars");

            //Load the content for the Scrolling background
            mScrollingBackground.LoadContent(this.Content);

            //Load the content for each platform
            foreach (Platform platform in platforms)
            {
                platform.LoadContent(this.Content);
            }

            //Load sound effect
            backgroundSong = Content.Load<Song>("Song");

            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        bool intersectsPlatform;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // set intersectsPlatform to false befor testing
            intersectsPlatform = false;

            foreach (Platform platform in platforms)
            {

                if (IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, platform.Size, platform.textureData))
                {
                    // intersectsPlatform is set true if player intersects with any platform
                    intersectsPlatform = true;
                    break;
                }
            }

            // hands intersectsPlatform to player class
            firstPlayerSprite.Update(gameTime, intersectsPlatform);

            monsterTrial.Update(gameTime);

            fireballEnergyBar.Update(gameTime);

            foreach (Platform platform in platforms)
                platform.Update(gameTime);

            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);

            //foreach (Platform platform in platforms)
            //    if (firstPlayerSprite.rectangle.isOnTopOf(platform.rectangle))
            //    {
            //        firstPlayerSprite.velocity.Y = 0f;
            //        firstPlayerSprite.hasJumped = false;
            //    }

            //use for if a fireball hits a monster
            foreach (Fireball f in firstPlayerSprite.mFireballs)
            {
                if (IntersectPixel(f.Size, f.textureData, monsterTrial.Size, monsterTrial.textureData))
                {
                    //Add code for it hit
                }
                else
                {
                    //Add code for not hit
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            mScrollingBackground.Draw(spriteBatch);

            foreach (Platform platform in platforms)
                platform.Draw(this.spriteBatch);

            firstPlayerSprite.Draw(this.spriteBatch);

            monsterTrial.Draw(this.spriteBatch);

            fireballEnergyBar.Draw(this.spriteBatch,graphics,firstPlayerSprite.fireballEnergyPercentage);

            //Kimi: For debug purpose
            spriteBatch.DrawString(font, firstPlayerSprite.Position.ToString(), new Vector2(10, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        static bool IntersectPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
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

        //We need to figure out changeInX and changeInY
        static Vector2 GenerateRandomLandLocation(int maxJumpHeight, float yPreviousLocation, int changeInX, int changeInY)
        {
            
            
            float slope = -(float)changeInY / (float)changeInX;
            int mjh = -maxJumpHeight;
            int cix = changeInX;
            int ciy = changeInY;
            float yprev = yPreviousLocation;

            int jumpHeight = mjh + 100;

            Random r = new Random();
            int randomY = r.Next((Convert.ToInt32(yPreviousLocation) + jumpHeight),700);
            int randomX = 0;
            int min = Convert.ToInt32(((float)randomY - yprev) / slope);
            int max = Convert.ToInt32((-1 * (float)randomY + yprev + 2 * (float)jumpHeight) / slope);

            if (randomY > yPreviousLocation)
            {
                randomX = r.Next(min, max-100);
            }
            else
            {
                randomX = r.Next(max,700-100);
            }
            return new Vector2(randomX + 1000, randomY);
        }


    }

}

//static class RectangleHelper
//{
//    const int penetrationMargin = 5;
//    public static bool isOnTopOf(this Rectangle r1, Rectangle r2)
//    {
//        return (r1.Bottom >= r2.Top - penetrationMargin &&
//            r1.Bottom <= r2.Top + 1 &&
//            r1.Right >= r2.Left + 5 &&
//            r1.Left <= r2.Right - 5);
//    }
//}

