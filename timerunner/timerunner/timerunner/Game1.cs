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
        List<Platform> platforms = new List<Platform>();
        SpriteFont font;

        //Sound
        Song backgroundSong;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            firstPlayerSprite = new Player(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            monsterTrial = new Monster(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
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
            // TODO: use this.Content to load your game content here
            firstPlayerSprite.LoadContent(this.Content);
            monsterTrial.LoadContent(this.Content);

            mScrollingBackground = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);
            mScrollingBackground.AddBackground("Background01");
            mScrollingBackground.AddBackground("Background02");
            mScrollingBackground.AddBackground("Background03");
            mScrollingBackground.AddBackground("Background04");
            mScrollingBackground.AddBackground("Background05");

            //Load the content for the Scrolling background
            mScrollingBackground.LoadContent(this.Content);

            //Load the content for the platform
            platforms.Add(new Platform(Content.Load<Texture2D>("Platform"), new Vector2(30, 400)));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platform"), new Vector2(350, 300)));
            platforms.Add(new Platform(Content.Load<Texture2D>("Platform"), new Vector2(700, 350)));

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

        bool touched;
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

            if (IntersectPixel(firstPlayerSprite.Size, firstPlayerSprite.textureData,
                monsterTrial.Size, monsterTrial.textureData))
            {
                touched = true;
            }
            else
                touched = false;

            // TODO: Add your update logic here
            firstPlayerSprite.Update(gameTime);
            monsterTrial.Update(gameTime);

            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);

            //foreach (Platform platform in platforms)
            //    if (firstPlayerSprite.rectangle.isOnTopOf(platform.rectangle))
            //    {
            //        firstPlayerSprite.velocity.Y = 0f;
            //        firstPlayerSprite.hasJumped = false;
            //    }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            if (touched)
                graphics.GraphicsDevice.Clear(Color.Red);
            else
                graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code hereC:\Users\Matt\Documents\GitHub\timerunner\timerunner\timerunner\timerunner\Game1.cs
            spriteBatch.Begin();
            mScrollingBackground.Draw(spriteBatch);            
            foreach (Platform platform in platforms)
                platform.Draw(spriteBatch);

            firstPlayerSprite.Draw(this.spriteBatch);
            monsterTrial.Draw(this.spriteBatch);
           
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

            for(int y = top; y<bottom; y++)
                for (int x = left; x < right; x++)
                {
                        Color c1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                        Color c2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                        if (c1.A != 0 && c2.A != 0)
                            return true;
                }

            return false;
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
}
