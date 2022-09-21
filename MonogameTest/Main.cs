﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HeliFireFighting
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        const float TARGET_FPS = 5f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        // Control Variables
        MouseState mouseState;
        KeyboardState keyboardState;

        public Main()
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
            TargetElapsedTime = TimeSpan.FromSeconds(1f / TARGET_FPS);
            IsFixedTimeStep = false;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

#if DEBUG
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
#else
            graphics.IsFullScreen = true;
            IsMouseVisible = false;
#endif

            graphics.ApplyChanges();

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //global = new Global(this, GraphicsDevice, spriteBatch);
            world = new World(GraphicsDevice,
                graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //global.LoadContent(Content);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update logic
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            world.Update(mouseState.X, mouseState.Y, mouseState.LeftButton == ButtonState.Pressed);

            //global.MouseStateGlobal = mouseState;
            //global.KeyboardStateGlobal = keyboardState;
            //global.ActiveScreen.Update(mouseState, keyboardState);
            //global.TubWasher.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            spriteBatch.Begin();
            world.Draw(spriteBatch);
            spriteBatch.End();
            //global.SpriteBatchGlobal.Begin();
            //global.ActiveScreen.Draw();
            //global.SpriteBatchGlobal.End();

            base.Draw(gameTime);
        }
    }
}