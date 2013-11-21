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
        HorizontallyScrollingBackground mScrollingBackgroundsky;
        randombackground mScrollingBackgroundlandscape;
        randombackground mScrollingBackgroundfront;

        Player firstPlayerSprite;
        Monster monsterTrial;
        FireballEnergyBar fireballEnergyBar;
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;
        Sprite startSprite, endSprite;

        //PlatForm 
        Platform currentPlatForm, outScreenPlatForm;

        //Sound
        Song backgroundSong;

        //Animation
        float frameTimer;
        float frameInterval;

        //Bool flag
        bool intersectsPlatform;
        bool monsterIntersectPlatform;

        // Player and Window Constants
        const float PLAYER_JUMP_HEIGHT = 300;
        const float PLAYER_INIT_HEIGHT = 500;
        const int WINDOW_HEIGHT = 700;
        const int WINDOW_WIDTH = 1000;

        //Game state
        public enum GameState
        {
            Gamebegin,
            Gaming,
            GameOver
        }

        public GameState gameState;

        public Game1()
        {
            gameState = GameState.Gamebegin;
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

            //Initialize letter sprites
            startSprite = new Sprite();
            startSprite.AssetName = "start";
            endSprite = new Sprite();
            endSprite.AssetName = "gameover";

            // Initialize Animation Properties
            frameTimer = 0;
            frameInterval = 80f;

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

            startSprite.texture = Content.Load<Texture2D>("start");
            startSprite.Position = new Vector2(WINDOW_WIDTH / 2 - startSprite.texture.Width / 2, WINDOW_HEIGHT / 2);
            endSprite.texture = Content.Load<Texture2D>("gameover");
            endSprite.Position = new Vector2(-100, -100);

            // Load Playercontent
            firstPlayerSprite.LoadContent(this.Content);

            fireballEnergyBar.LoadContent(this.Content);

            // TODO: Load any ResourceManagementMode.Automatic content

            IsMouseVisible = true;


            mScrollingBackgroundsky = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);

            mScrollingBackgroundsky.AddBackground("sunrise");
            mScrollingBackgroundsky.AddBackground("daywclouds");
            mScrollingBackgroundsky.AddBackground("dusk");
            mScrollingBackgroundsky.AddBackground("nightwstars");

            mScrollingBackgroundlandscape = new randombackground(this.GraphicsDevice.Viewport);

            mScrollingBackgroundlandscape.AddBackground("level2a");
            mScrollingBackgroundlandscape.AddBackground("level2b");
            mScrollingBackgroundlandscape.AddBackground("level2c");
            mScrollingBackgroundlandscape.AddBackground("level2d");
            mScrollingBackgroundlandscape.AddBackground("level2e");
            mScrollingBackgroundlandscape.AddBackground("level2f");
            mScrollingBackgroundlandscape.AddBackground("level2g");

            mScrollingBackgroundfront = new randombackground(this.GraphicsDevice.Viewport);
            mScrollingBackgroundfront.AddBackground("level3a");
            mScrollingBackgroundfront.AddBackground("level3b");
            mScrollingBackgroundfront.AddBackground("level3c");
            mScrollingBackgroundfront.AddBackground("level3d");
            mScrollingBackgroundfront.AddBackground("level3e");
            mScrollingBackgroundfront.AddBackground("level3f");
            mScrollingBackgroundfront.AddBackground("level3g");
            mScrollingBackgroundfront.AddBackground("level3h");

            //Load the content for the Scrolling background
            mScrollingBackgroundsky.LoadContent(this.Content);
            mScrollingBackgroundlandscape.LoadContent(this.Content);
            mScrollingBackgroundfront.LoadContent(this.Content);

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

            HelpClass.UpdateMouse();
            if ((GamePad.GetState(PlayerIndex.One).Buttons.BigButton == ButtonState.Pressed || HelpClass.checkMouseClickOnSprite(startSprite.Position, startSprite.texture)) && gameState == GameState.Gamebegin)
            {
                gameState = GameState.Gaming;
                startSprite.Position = new Vector2(-100, -100);
            }

            // set intersectsPlatform to false befor testing
            intersectsPlatform = false;


            foreach (Platform platform in platforms)
            {

                if (HelpClass.IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, platform.Size, platform.textureData))
                {
                    // intersectsPlatform is set true if player intersects with any platform
                    intersectsPlatform = true;
                    break;
                }
            }

            if (gameState == GameState.Gaming)
            {
                // hands intersectsPlatform to player class
                firstPlayerSprite.Update(gameTime, intersectsPlatform);

                RandomMonsterGenerator();

                if (monsterVisible == true)
                {
                    foreach (Platform platform in platforms)
                    {

                        {
                            if (HelpClass.IntersectPixel(monsterTrial.Size, monsterTrial.textureData, platform.Size, platform.textureData))
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
                            if (HelpClass.IntersectPixel(f.Size, f.textureData, monsterTrial.Size, monsterTrial.textureData))
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
                    if (HelpClass.IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, monsterTrial.Size, monsterTrial.textureData))
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
                //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
                mScrollingBackgroundsky.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);
                mScrollingBackgroundlandscape.Update(gameTime, 210, randombackground.HorizontalScrollDirection.Left);
                mScrollingBackgroundfront.Update(gameTime, 260, randombackground.HorizontalScrollDirection.Left);

                //generate the platforms
                foreach (Platform platform in platforms)
                {
                    platform.Update(gameTime);
                    if ((currentPlatForm.Position.X + currentPlatForm.texture.Width) < 1000)
                    {
                        Platform temp = currentPlatForm;
                        outScreenPlatForm.Position = HelpClass.GenerateRandomLandLocation(300, currentPlatForm.Position.Y, 400, Convert.ToInt32(1.9f * 300));
                        //outScreenPlatForm.Position = GenerateRandomLandLocation(Convert.ToInt32(firstPlayerSprite.MAX_JUMP_HEIGHT), currentPlatForm.PlatformSpeed(), Convert.ToInt32(currentPlatForm.Position.Y), Convert.ToInt32(firstPlayerSprite.MOVE_UP));
                        currentPlatForm = outScreenPlatForm;

                        outScreenPlatForm = temp;
                        //platform.stop();
                    }
                }

                // check if player state is die
                if (firstPlayerSprite.mCurrentState == Player.State.Die && gameState == GameState.Gaming)
                {
                    gameState = GameState.GameOver;
                    endSprite.Position = new Vector2(WINDOW_WIDTH / 2 - endSprite.texture.Width / 2, WINDOW_HEIGHT / 2);
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

            mScrollingBackgroundsky.Draw(spriteBatch);
            mScrollingBackgroundlandscape.Draw(spriteBatch);
            mScrollingBackgroundfront.Draw(spriteBatch);

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

            startSprite.Draw(spriteBatch);

            endSprite.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
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

