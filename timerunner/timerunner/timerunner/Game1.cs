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

            // TODO: use this.Content to load your game content here
            firstPlayerSprite.LoadContent(this.Content);

            mScrollingBackground = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);
            mScrollingBackground.AddBackground("Background01");
            mScrollingBackground.AddBackground("Background02");
            mScrollingBackground.AddBackground("Background03");
            mScrollingBackground.AddBackground("Background04");
            mScrollingBackground.AddBackground("Background05");

            //Load the content for the Scrolling background
            mScrollingBackground.LoadContent(this.Content);
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

            // TODO: Add your update logic here
            firstPlayerSprite.Update(gameTime);

            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            mScrollingBackground.Draw(spriteBatch);
            firstPlayerSprite.Draw(this.spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
