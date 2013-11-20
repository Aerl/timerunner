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
        int speedOfGround = 200;

        float randomStarter = 0;
        bool monsterVisible = false;

        //Create a Horizontally scrolling background
        HorizontallyScrollingBackground mScrollingBackground;

        Player firstPlayerSprite;
        Monster monsterTrial;
        FireballEnergyBar fireballEnergyBar;
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;

        //PlatForm 
        Platform currentPlatForm, outScreenPlatForm;

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

            fireballEnergyBar = new FireballEnergyBar();

            // Initialize Platforms
            currentPlatForm = new Platform("Platform", new Vector2(30, 400));
            platforms.Add(currentPlatForm);
            outScreenPlatForm = new Platform("Platform", new Vector2(1300, -350));
            platforms.Add(outScreenPlatForm);

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

            fireballEnergyBar.LoadContent(this.Content);

            // TODO: Load any ResourceManagementMode.Automatic content


            mScrollingBackground = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);

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

            //MediaPlayer.Play(backgroundSong);
            //MediaPlayer.IsRepeating = true;
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
        bool monsterIntersectPlatform;
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

            RandomMonsterGenerator();


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

            if (monsterVisible == true)
            {
                foreach (Platform platform in platforms)
                {

                    {
                        if (IntersectPixel(monsterTrial.Size, monsterTrial.textureData, platform.Size, platform.textureData))
                        {
                            // intersectsPlatform is set true if monster intersects with any platform
                            monsterIntersectPlatform = true;
                            break;
                        }
                    }
                }
                //use for if a fireball hits a monster
                foreach (Fireball f in firstPlayerSprite.mFireballs)
                {
                     {
                        if (IntersectPixel(f.Size, f.textureData, monsterTrial.Size, monsterTrial.textureData))
                        {
                            monsterTrial.Hit();
                            f.Position.X = 2000;
                            f.Size.X = 2000;
                        }
                        else
                        {
                            //Add code for not hit
                        }
                    }
                }
                if (IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, monsterTrial.Size, monsterTrial.textureData))
                {
                    //monsterTrial.Hit();
                    //f.Position.X = 1200;
                }
                monsterTrial.Update(gameTime, monsterIntersectPlatform);

                if (monsterTrial.Size.X < -200 || monsterTrial.Size.Y > 1500)
                {
                    monsterTrial = null;
                    monsterVisible = false;
                    monsterIntersectPlatform = false;
                }
            }

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



            //generate the platforms
            foreach (Platform platform in platforms)
            {
                platform.Update(gameTime);
                if ((currentPlatForm.Position.X + currentPlatForm.texture.Width) < 1000)
                {
                    Platform temp = currentPlatForm;
                    outScreenPlatForm.Position = GenerateRandomLandLocation(300, currentPlatForm.Position.Y, 400, Convert.ToInt32(1.9f * 300));
                    //outScreenPlatForm.Position = GenerateRandomLandLocation(Convert.ToInt32(firstPlayerSprite.MAX_JUMP_HEIGHT), currentPlatForm.PlatformSpeed(), Convert.ToInt32(currentPlatForm.Position.Y), Convert.ToInt32(firstPlayerSprite.MOVE_UP));
                    currentPlatForm = outScreenPlatForm;

                    outScreenPlatForm = temp;
                    //platform.stop();
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

            if (monsterVisible == true)
            {
                monsterTrial.Draw(this.spriteBatch);
            }

            fireballEnergyBar.Draw(this.spriteBatch,graphics,firstPlayerSprite.fireballEnergyPercentage);

            //Kimi: For debug purpose
            spriteBatch.DrawString(font, firstPlayerSprite.Position.ToString(), new Vector2(10, 10), Color.White);
            if (monsterVisible)
            {
                spriteBatch.DrawString(font, monsterTrial.Position.ToString(), new Vector2(10, 50), Color.White);
            }

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
        static Vector2 GenerateRandomLandLocation(int max, float yPreviousLocation, int changeInX, int changeInY)
        {
            double slope = (double)changeInY * -1 / (double)changeInX;
            int yPrev = Convert.ToInt32(yPreviousLocation);
            int randomY=0;
            int randomX=0;
            int maxJumpHeight = max - 30;
            while (randomX < 8
                
                0)
            {
                Random r = new Random();
                if (yPrev - maxJumpHeight < 100)
                    randomY = r.Next(100, 600);
                else
                    randomY = r.Next(yPrev - maxJumpHeight, 600);
                int x1 = Convert.ToInt32((randomY - yPrev) / slope);
                int x2 = Convert.ToInt32((randomY - yPrev + 2 * maxJumpHeight) / (-1 * slope));
                if (randomY > yPrev)
                {
                    randomX = r.Next(0, x2);
                }
                else
                {
                    randomX = r.Next(x1, x2);
                }
            }
            return new Vector2(randomX + 1000, randomY);
        }

        public void GenerateRandomMonster()
        {
            // Initialize Monters
            monsterTrial = new Monster(speedOfGround);
            
            // Load Monstercontent
            monsterTrial.LoadContent(this.Content);
            monsterVisible = true;
        }

        private void RandomMonsterGenerator()
        {
            //Random Generation of Monsters
            if (randomStarter < 300 && monsterVisible == false)
            {
                randomStarter++;
            }
            else if (monsterVisible == false)
            {
                Random r = new Random();
                int possibleGenerationNumber = r.Next(0, 300);
                if (possibleGenerationNumber == 50)
                {
                    GenerateRandomMonster();
                }
            }
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

