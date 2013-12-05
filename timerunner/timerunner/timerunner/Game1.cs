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
        public SpriteBatch spriteBatch;

        public int platformIntersectY=0;
        float randomStarter = 0;
        bool monsterVisible = false;

        //Create a Horizontally scrolling background
        HorizontallyScrollingBackground mScrollingBackgroundsky;
        randombackground mScrollingBackgroundlandscape;
        randombackground mScrollingBackgroundfront;

        Runner runner;
        Monster monsterTrial;
        FireballEnergyBar fireballEnergyBar;
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;
        Sprite startSprite, endSprite;
        Random randomMonsterNumber = new Random();

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
        const int PLAYER_JUMP_HEIGHT = 300;
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

        //Game Speed
        public static int gameSpeed = 200;
        public static float gameCounter = 0;
        const int GAME_SPEED_INCREASE = 100;
        const int GAME_COUNTER_RESET = 500;

        //add for animation
        List<GameEntity> entities = new List<GameEntity>();

        public List<GameEntity> Entities
        {
            get { return entities; }
            set { entities = value; }
        }

        private static Game1 instance;

        public static Game1 Instance
        {
            get
            {
                return instance;
            }
        }

        public Game1()
        {
            gameState = GameState.Gamebegin;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            Content.RootDirectory = "Content";
            instance = this; //add for animation
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            fireballEnergyBar = new FireballEnergyBar();

            // Initialize Platforms
            currentPlatForm = new Platform("Platform", new Vector2(30, 400),gameSpeed);
            platforms.Add(currentPlatForm);
            outScreenPlatForm = new Platform("Platform", new Vector2(1300, -350),gameSpeed);
            platforms.Add(outScreenPlatForm);

            //Initialize letter sprites
            startSprite = new Sprite();
            startSprite.AssetName = "start";
            endSprite = new Sprite();
            endSprite.AssetName = "gameover";

            // Initialize Animation Properties
            frameTimer = 0;
            frameInterval = 80f;

            runner = new Runner();
            runner.Position.X += 100;
            entities.Add(runner);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            IsMouseVisible = true;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");

            startSprite.texture = Content.Load<Texture2D>("start");
            startSprite.Position = new Vector2(WINDOW_WIDTH / 2 - startSprite.texture.Width / 2, WINDOW_HEIGHT / 2);
            endSprite.texture = Content.Load<Texture2D>("gameover");
            endSprite.Position = new Vector2(-100, -100);

            
            fireballEnergyBar.LoadContent(this.Content);

            // TODO: Load any ResourceManagementMode.Automatic content


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

            // Create a new SpriteBatch, which can be used to draw textures.
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].LoadContent();
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
            if (gameState == GameState.Gaming)
            {
                gameCounter++;
                if (gameCounter > GAME_COUNTER_RESET)
                {
                    gameSpeed += GAME_SPEED_INCREASE;
                    gameCounter = 0;
                }
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            HelpClass.UpdateMouse();
            if ((GamePad.GetState(PlayerIndex.One).Buttons.BigButton == ButtonState.Pressed || HelpClass.checkMouseClickOnSprite(startSprite.Position, startSprite.texture)))
            {
                
                runner.currentState = Runner.State.Falling;
                gameState = GameState.Gaming;
                startSprite.Position = new Vector2(-100, -100);
                endSprite.Position = new Vector2(-100, -100);
            }

            // set intersectsPlatform to false befor testing
            intersectsPlatform = false;


            foreach (Platform platform in platforms)
            {                    
                if (HelpClass.IsOnTopOf(runner, platform))
                {
                    intersectsPlatform = true;
                    platformIntersectY = platform.Size.Y;
                    break;
                }

                //Kimi: I added new hit detection method above for new Runner.cs character, but this one still work for old Player.cs character.
                //if (HelpClass.TopHitDetection(platform, firstPlayerSprite))
                //{
                //    if (HelpClass.IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, platform.Size, platform.textureData))
                //    {
                //        // intersectsPlatform is set true if player onPlatform with any platform
                //        intersectsPlatform = true;
                //        platformIntersectY = platform.Size.Y;
                //        break;
                //    }
                //}
                //else
                //    if (HelpClass.IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData, platform.Size, platform.textureData))
                //    {
                //        // intersectsPlatform is set true if player onPlatform with any platform
                //        GameOver();
                //        break;
                //    }

            }

            if (gameState == GameState.Gaming)
            {
                // hands intersectsPlatform to player class
                runner.intersects = intersectsPlatform;

                RandomMonsterGenerator();

                if (monsterVisible == true)
                {
                    foreach (Platform platform in platforms)
                    {
                        if (HelpClass.IntersectPixel(monsterTrial.Size, monsterTrial.textureData, platform.Size, platform.textureData))
                        {
                            // intersectsPlatform is set true if monster onPlatform with any platform
                            monsterIntersectPlatform = true;
                            break;
                        }
                    }
                    //use for if a fireball hits a monster
                    foreach (Fireball f in runner.mFireballs)
                    {
                        if (HelpClass.IntersectPixel(f.Size, f.textureData, monsterTrial.Size, monsterTrial.textureData))
                        {
                            monsterTrial.Hit(ref runner.score);
                            f.Position.X = 2000;
                            f.Size.X = 2000;
                        }
                        else
                        {
                            //Add code for not hit
                        }
                    }
                    if (monsterTrial.mCurrentState != Monster.State.Dead)
                    {
                        Rectangle Size = new Rectangle((int)runner.Position.X, (int)runner.Position.Y, (int)(runner.Sprite.Width), (int)(runner.Sprite.Height));
                        if (HelpClass.IntersectPixel(Size, new Color[runner.Sprite.Width * runner.Sprite.Height], monsterTrial.Size, monsterTrial.textureData))
                        {
                            if(runner.SwordAttack==false)
                            {
                                GameOver();
                            }
                            else
                            {
                                monsterTrial.mCurrentState = Monster.State.Dead;
                            }
                        }
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
                        outScreenPlatForm.Position = HelpClass.GenerateRandomLandLocation(PLAYER_JUMP_HEIGHT, currentPlatForm.Position.Y, gameSpeed, 900);
                        //outScreenPlatForm.Position = GenerateRandomLandLocation(Convert.ToInt32(firstPlayerSprite.MAX_JUMP_HEIGHT), currentPlatForm.PlatformSpeed(), Convert.ToInt32(currentPlatForm.Position.Y), Convert.ToInt32(firstPlayerSprite.MOVE_UP));
                        currentPlatForm = outScreenPlatForm;

                        outScreenPlatForm = temp;
                        //platform.stop();
                    }
                }

                // check if player state is die
                if (runner.currentState == Runner.State.Die && gameState == GameState.Gaming)
                {
                    GameOver();
                }

                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        private void GameOver()
        {
            gameState = GameState.GameOver;
            endSprite.Position = new Vector2(WINDOW_WIDTH / 2 - endSprite.texture.Width / 2, WINDOW_HEIGHT / 2);
            //startSprite.Position = new Vector2(WINDOW_WIDTH / 2 - startSprite.texture.Width / 2, WINDOW_HEIGHT / 2 - 40);
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
            
            if (monsterVisible == true)
            {
                monsterTrial.Draw(this.spriteBatch);
            }

            fireballEnergyBar.Draw(this.spriteBatch,graphics,runner.fireballEnergyPercentage);

            //Kimi: For debug purpose
            //spriteBatch.DrawString(font, firstPlayerSprite.Position.ToString(), new Vector2(10, 10), Color.White);
            //if (monsterVisible)
            //{
            //    spriteBatch.DrawString(font, monsterTrial.Position.ToString(), new Vector2(10, 50), Color.White);
            //}

            spriteBatch.DrawString(font,"Score: " + runner.score.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, "Position: " + runner.Position.Y.ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(font, "State: " + runner.currentState.ToString(), new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(font, "runner.intersects: " + (runner.intersects).ToString(), new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(font, "runner.melee: " + (runner.SwordAttack).ToString(), new Vector2(10, 90), Color.White);
            startSprite.Draw(spriteBatch);

            endSprite.Draw(spriteBatch);

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(gameTime);
            }

            foreach (Fireball fireball in runner.mFireballs)
            {
                if (fireball.Visible == true)
                {
                    fireball.Draw(this.spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void GenerateRandomMonster()
        {
            // Initialize Monters
            int random = randomMonsterNumber.Next(0, 100);
            if(random>50)
            {
                monsterTrial = new Dragon(gameSpeed);
            }
            else
            {
                monsterTrial = new Walrus(gameSpeed);
            }
            
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


