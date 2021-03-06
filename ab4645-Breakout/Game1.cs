﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace ab4645_Breakout
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
#if (!ARCADE)
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
#else
		public override string GameDisplayName { get { return "ab4645-Breakout"; } }
#endif
        GameplayManager gm;
        public static Random rand = new Random();


        public Game1()
        {
#if (!ARCADE)
            graphics = new GraphicsDeviceManager(this);
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if (!ARCADE)
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
#endif

            //Textures
            AssetManager.AddTexture("pixel", Content.Load<Texture2D>("pixel"));
            AssetManager.AddTexture("paddles", Content.Load<Texture2D>("paddles"));
            AssetManager.AddTexture("block", Content.Load<Texture2D>("block"));
            //AssetManager.AddTexture("ball", Content.Load<Texture2D>("ball_spritesheet"));
            AssetManager.AddTexture("balls", Content.Load<Texture2D>("balls"));
            AssetManager.AddTexture("softparticle", Content.Load<Texture2D>("softparticle"));
            AssetManager.AddTexture("powerup", Content.Load<Texture2D>("powerup"));
            AssetManager.AddTexture("level1bg", Content.Load<Texture2D>("level1bg"));
            AssetManager.AddTexture("heart", Content.Load<Texture2D>("heart"));
            AssetManager.AddTexture("heart_blue", Content.Load<Texture2D>("heart_blue"));
            AssetManager.AddTexture("gun", Content.Load<Texture2D>("gun"));

            //Fonts
            AssetManager.AddFont("main", Content.Load<SpriteFont>("MainFont"));

            //Sound effects
            AssetManager.AddSound("bounce", Content.Load<SoundEffect>("bounce"));
            AssetManager.AddSound("death", Content.Load<SoundEffect>("death"));
            AssetManager.AddSound("powerup", Content.Load<SoundEffect>("powerup_get"));

            //Music
           
            gm = new GameplayManager(Content.RootDirectory);

            
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
#if (!ARCADE)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red)) {
            //    int breakme;
            //}
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Yellow))
            //{
            //    int breakme;
            //}
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Green))
            //{
            //    int breakme;
            //}
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue))
            //{
            //    int breakme;
            //}
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.A))
            //{
            //    int breakme;
            //}
            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.B))
            //{
            //    int breakme;
            //}

            gm.Update(gameTime);

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(AssetManager.GetTexture("level1bg"), Vector2.Zero, Color.White);
            gm.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
