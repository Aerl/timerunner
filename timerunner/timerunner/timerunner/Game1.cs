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
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public int platformIntersectY=0;
        float randomStarter = 0;
        

        //Create a Horizontally scrolling background
        HorizontallyScrollingBackground mScrollingBackgroundsky;
        randombackground mScrollingBackgroundlandscape;
        randombackground mScrollingBackgroundfront;
        Vector2 scoreLocation = new Vector2(-100, -100);
        InputManager im = new InputManager();
        Runner runner;

        //Monster variables
        Monster monsterOne;
        Monster secondMonster;
        Monster thirdMonster;
        bool monsterVisible = false;
        bool secondMonsterVisible = false;
        bool thirdMonsterVisible = false;
        const int secondMonsterScore = 8000;
        const int thirdMonsterScore = 16000;

        FireballEnergyBar fireballEnergyBar;
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;
        SpriteFont font2;
        Sprite startSprite, endSprite, endSpriteScreen;
        Random randomMonsterNumber = new Random();

        //PlatForm 
        Platform currentPlatForm, outScreenPlatForm;

        //Sound
        Song backgroundSong1;
        Song backgroundSong2;
        Song backgroundSong3;
        Song currentSong;

        //Animation
        float frameTimer;
        float frameInterval;

        //Bool flag
        bool intersectsPlatform;

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
        public static int gameSpeed = 300;
        public static float gameCounter = 0;
        const int GAME_SPEED_INCREASE = 25;
        const int GAME_COUNTER_RESET = 750;
        const float INTERVAL_INCREASE = .01f;
        const int MAX_GAME_SPEED = 650;

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
            startSprite.AssetName = "startscreen";
            endSprite = new Sprite();
            endSprite.AssetName = "gameover";
            endSpriteScreen = new Sprite();
            endSpriteScreen.AssetName = "gameoverScreen";

            // Initialize Animation Properties
            frameTimer = 0;
            frameInterval = 80f;

            runner = new Runner();
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
            font2 = Content.Load<SpriteFont>("Monotype Corsiva");

            startSprite.texture = Content.Load<Texture2D>("startscreen");
            startSprite.Position = new Vector2(0,0);
            endSprite.texture = Content.Load<Texture2D>("gameover");
            endSprite.Position = new Vector2(-100, -100);
            endSpriteScreen.texture = Content.Load<Texture2D>(endSpriteScreen.AssetName);
            endSpriteScreen.Position = new Vector2(-1000, -1000);

            
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
            backgroundSong1 = Content.Load<Song>("Song1");
            backgroundSong2 = Content.Load<Song>("Song2");
            backgroundSong3 = Content.Load<Song>("Song3");
            currentSong = backgroundSong1;

            MediaPlayer.Play(currentSong);
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            im.SetCurrentInputState();
            if (gameState == GameState.Gaming && gameSpeed <= MAX_GAME_SPEED)
            {
                gameCounter++;
                if (gameCounter > GAME_COUNTER_RESET )
                {
                    gameSpeed += GAME_SPEED_INCREASE;
                    runner.walkingInterval = (float)(runner.walkingInterval - INTERVAL_INCREASE);
                    gameCounter = 0;
                    runner.fireballEnergyIncrease += .00025;
                }
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            HelpClass.UpdateMouse();
            if (gameState == GameState.GameOver || gameState == GameState.Gamebegin)
            {
                if ((GamePad.GetState(PlayerIndex.One).Buttons.BigButton == ButtonState.Pressed || HelpClass.checkMouseClickOnSprite(startSprite.Position, startSprite.texture) || HelpClass.checkMouseClickOnSprite(endSpriteScreen.Position, endSpriteScreen.texture) || PressedFireball()))
                {
                    runner.fireballEnergyPercentage = 0;
                    runner.walkingInterval = 0.2f;
                    runner.Interval = 0.2f;
                    gameSpeed = 300;
                    runner.Position = new Vector2(220, 170);
                    currentPlatForm.Position = new Vector2(30, 400);
                    outScreenPlatForm.Position = new Vector2(1300, -350);
                    endSpriteScreen.Position = new Vector2(-1000, -1000);
                    scoreLocation = new Vector2(-100, -100);
                    runner.score = 0;
                    runner.currentState = Runner.State.Falling;

                    if (monsterOne != null)
                    {
                        monsterOne.Position = new Vector2(0, 1500);
                        monsterVisible = false;
                    }
                    if (secondMonster != null)
                    {
                        secondMonster.Position = new Vector2(0, 1500);
                        monsterVisible = false;
                    }
                    if (thirdMonster != null)
                    {
                        thirdMonster.Position = new Vector2(0, 1500);
                        monsterVisible = false;
                    }

                    gameState = GameState.Gaming;
                    startSprite.Position = new Vector2(-1000, -1000);
                    endSprite.Position = new Vector2(-100, -100);


                }
            }

            // set intersectsPlatform to false befor testing
            intersectsPlatform = false;


            foreach (Platform platform in platforms)
            {                    
                if (HelpClass.IsRunnerOnTopOf(runner, platform))
                {
                    intersectsPlatform = true;
                    platformIntersectY = platform.Size.Y;
                    break;
                }
            }

            if (gameState == GameState.Gaming)
            {
                // hands intersectsPlatform to player class
                runner.intersects = intersectsPlatform;

                RandomMonsterGenerator();

                if (monsterVisible == true)
                {
                    MonsterMethods(gameTime,monsterOne,ref monsterVisible);
                }

                if (secondMonsterVisible == true)
                {
                    MonsterMethods(gameTime,secondMonster, ref secondMonsterVisible);
                }

                if (thirdMonsterVisible == true)
                {
                    MonsterMethods(gameTime, thirdMonster, ref thirdMonsterVisible);
                }

                fireballEnergyBar.Update(gameTime);

                foreach (Platform platform in platforms)
                     platform.Update(gameTime);

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
                        outScreenPlatForm.Position = HelpClass.GenerateRandomLandLocation(PLAYER_JUMP_HEIGHT, currentPlatForm.Position.Y, gameSpeed, (int)(runner.MOVE_DOWN * 1 / (float)gameTime.ElapsedGameTime.TotalSeconds));
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

                if (runner.score > secondMonsterScore && currentSong.Equals(backgroundSong1))
                {
                    MediaPlayer.Stop();
                    currentSong = backgroundSong2;
                    MediaPlayer.Play(currentSong);
                }
                else if (runner.score > thirdMonsterScore && !currentSong.Equals(backgroundSong3))
                {
                        MediaPlayer.Stop();
                        currentSong = backgroundSong3;
                        MediaPlayer.Play(currentSong);
                }
                else if (!currentSong.Equals(backgroundSong1) && runner.score < secondMonsterScore)
                {
                    MediaPlayer.Stop();
                    currentSong = backgroundSong1;
                    MediaPlayer.Play(currentSong);
                }
            }
            base.Update(gameTime);
        }

        private void MonsterMethods(GameTime gameTime, Monster monsterTrial, ref bool monsterVisible)
        {
              foreach (Platform platform in platforms)
              {
                    if (HelpClass.IsMonsterInBounds(monsterTrial, platform))
                    {
                        if (HelpClass.IntersectPixel(monsterTrial.Size, monsterTrial.textureData, platform.Size, platform.textureData))
                        {
                            monsterTrial.intersects = true;
                            break;
                        }
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
                        if (HelpClass.IntersectRunner(runner,monsterTrial))
                        {
                            if(runner.SwordAttack==false)
                            {
                                GameOver();
                            }
                            else
                            {
                                monsterTrial.HitByMelee(ref runner.score);
                            }
                        }
                    }
                    monsterTrial.Update(gameTime);

                    if (monsterTrial.Size.X < -200 || monsterTrial.Size.Y > 1500)
                    {
                        monsterTrial = null;
                        monsterVisible = false;
                    }
        }

        private void GameOver()
        {
            gameState = GameState.GameOver;
            endSpriteScreen.Position = new Vector2(WINDOW_WIDTH / 2, 0);
            scoreLocation = new Vector2(WINDOW_WIDTH / 2 + endSpriteScreen.texture.Width / 2 - 30, WINDOW_HEIGHT / 2 + 80);
            
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

            fireballEnergyBar.Draw(this.spriteBatch,graphics,runner.fireballEnergyPercentage);
            HelpClass.CreateRunnerRectangle(runner);

            if (monsterVisible == true)
            {
                monsterOne.Draw(this.spriteBatch);
            }

            if (secondMonsterVisible == true)
            {
                secondMonster.Draw(this.spriteBatch);
            }

            if (thirdMonsterVisible == true)
            {
                thirdMonster.Draw(this.spriteBatch);
            }

            spriteBatch.DrawString(font,"Score: " + runner.score.ToString(), new Vector2(10, 10), Color.White);
            

            startSprite.Draw(spriteBatch);

            endSpriteScreen.Draw(spriteBatch);
            endSprite.Draw(spriteBatch);
            spriteBatch.DrawString(font2, "Score: " + runner.score.ToString(), scoreLocation, Color.Black);

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


        public void GenerateRandomMonster(ref Monster monster, ref bool monsterVisible)
        {
            // Initialize Monters
            int random = randomMonsterNumber.Next(0, 100);
            if(random>50)
            {
                monster = new Walrus(gameSpeed);
            }
            else
            {
                monster = new Dragon(gameSpeed);
            }
            
            // Load Monstercontent
            monster.LoadContent(this.Content);
            monsterVisible = true;
        }

        private void RandomMonsterGenerator()
        {
            //Random Generation of Monsters
            if (randomStarter < 300)
            {
                randomStarter++;
            }
            else if (monsterVisible == false)
            {
                Random r = new Random();
                int possibleGenerationNumber = r.Next(0, 300);
                if (possibleGenerationNumber == 50)
                {
                    GenerateRandomMonster(ref monsterOne, ref monsterVisible);
                }
            }


            if (randomStarter>=300 && secondMonsterVisible == false && runner.score>secondMonsterScore)
            {
                Random r = new Random();
                int possibleGenerationNumber = r.Next(0, 300);
                if (possibleGenerationNumber == 60)
                {
                    GenerateRandomMonster(ref secondMonster, ref secondMonsterVisible);
                }
            }
            if (randomStarter >= 300 && thirdMonsterVisible == false && runner.score > thirdMonsterScore)
            {
                Random r = new Random();
                int possibleGenerationNumber = r.Next(0, 300);
                if (possibleGenerationNumber == 70)
                {
                    GenerateRandomMonster(ref thirdMonster, ref thirdMonsterVisible);
                }
            }
        }
        public bool PressedJump()
        {
            return im.IsPressed(Keys.Up, Buttons.A);
        }

        public bool PressedFireball()
        {
            return im.IsPressed(Keys.Space, Buttons.B);
        }

        public bool PressedMelee()
        {
            return im.IsPressed(Keys.A, Buttons.X);
        }
    }

}


