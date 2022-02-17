using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace SimpleMovementJump
{
    /// <summary>
    /// Simple Movement For Jumping
    /// Uses a simple class called KeyboardHandler for input
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 instance;
        public static GraphicsDeviceManager graphics;
        public static float DeltaTime;
        public static double Time;

        SpriteBatch spriteBatch;
        KeyboardHandler keyboardHandler;

        private List<Sprite> sprites;
        private List<Sprite> addedSprites; //newly added sprites

        SpriteFont font;
        string OutputData;
        static Random rand;
        Texture2D background;
        

        public Game1()
        {
            if (instance != null)
                return;
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            sprites = new List<Sprite>();
            addedSprites = new List<Sprite>();
            rand = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
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

            background = Content.Load<Texture2D>("XPBackground");
            
            /*Sprite goofyHead = new Sprite();
            goofyHead.sprite = Content.Load<Texture2D>("goofyheadd");
            MovementComponent movementComponent = new MovementComponent();
            movementComponent.ParticleTex = Content.Load<Texture2D>("particle");
            goofyHead.AddComponent(movementComponent);
            goofyHead.position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);*/

            Sprite ciggy = new Sprite();
            ciggy.SetSprite(Content.Load<Texture2D>("ciggyTransparent"), Sprite.Origin.MiddleCenter);
            MovementComponent movementComponent2 = new MovementComponent();
            movementComponent2.Accel = 50;
            movementComponent2.MaxSpeed = 350;
            movementComponent2.ParticleAmount = 10;
            movementComponent2.ParticleTex = Content.Load<Texture2D>("particle");
            ciggy.AddComponent(movementComponent2);
            ciggy.position = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);



            font = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            AddRegisteredSprites();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardHandler.Update();

            //Elapsed time since last update
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time = gameTime.TotalGameTime.TotalSeconds;

            for (int i = 0; i < sprites.Count; i++)
            {
                if(sprites[i] != null)
                    sprites[i].Update();
                if (sprites[i].DeleteMe)
                    sprites.RemoveAt(i);
                    
            }
            

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0,0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f );

            foreach(Sprite s in sprites)
            {
                if(s.sprite != null)
                {
                    spriteBatch.Draw(s.sprite, new Rectangle((int)s.position.X, (int)s.position.Y, s.sprite.Width, s.sprite.Height), null, Color.White, s.rotation, s.origin, SpriteEffects.None, s.depth);
                }
            }

            //spriteBatch.DrawString(font, OutputData , new Vector2(10, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddRegisteredSprites()
        {
            if(addedSprites.Count > 0)
            {
                foreach(Sprite s in addedSprites)
                {
                    sprites.Add(s);
                }
                addedSprites.Clear();
            }
        }

        public void RegisterSprite(Sprite newSprite)
        {
            addedSprites.Add(newSprite);
        }


        public static float RandomRange(int min, int max)
        {
            return (float)rand.Next(min, max);
        }
    }
}
