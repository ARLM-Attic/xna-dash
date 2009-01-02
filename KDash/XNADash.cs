#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNADash.Animation;
using XNADash.Level;
using XNADash.Sprites;

#endregion

namespace XNADash
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNADash : Game
    {
        public static GraphicsDeviceManager graphics;
        private EnemySprite butterflySprite;
        public Camera2D camera;
        private string collisionDebugString;
        private KeyboardState currentKeyboardState;
        private Level.Level currentLevel;
        private Vector2 displaySize;
        private Sprite fireflySprite;
        public SpriteFont font;
        private bool playerIsDead;
        private MovingSprite playerSprite;
        private SceneGraph sceneGraph;
        private SpriteBatch spriteBatch;
        public bool visibilityChanged;
        private HUD gameHUD;
        private int score;
        private DateTime levelTime;
        private Vector2 HUDPosition;

        public Level.Level CurrentLevel
        {
            get { return currentLevel; }
        }

        /// <summary>
        /// The time passed since the level started
        /// </summary>
        public DateTime LevelTime
        {
            get { return levelTime; }
        }

        /// <summary>
        /// The players current score
        /// </summary>
        public int Score
        {
            get { return score; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public XNADash()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
            levelTime = DateTime.Now;
            score = 0;
            HUDPosition = new Vector2(0, graphics.PreferredBackBufferHeight - 20);

            playerIsDead = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.DeviceReset += OnGraphicsComponentDeviceReset;
            OnGraphicsComponentDeviceReset(this, new EventArgs());

            // Add Frame per second counter
            Components.Add(new FrameRateCounter(this, new Vector2(800, 0)));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("david");

            currentLevel = new Level.Level(this);
            CurrentLevel.LoadLevel("level\\level01.lvl");
            //level1.LoadLevel("level\\TESTLEVEL.lvl");
            //gameLevels.Add(level1);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Prepare scene
            sceneGraph = new SceneGraph(this, spriteBatch);

            // Set up player sprite
            playerSprite = new MovingSprite(this, Content.Load<Texture2D>("player"), CurrentLevel.StartPosition);
            // Set up butterfly sprite animation
            butterflySprite = new EnemySprite(this, Content.Load<Texture2D>("butterfly"), new Vector2(2100, 900));
            // Set up butterfly sprite animation
            fireflySprite = new Sprite(this, Content.Load<Texture2D>("firefly"), new Vector2(300, 400));

            // Set Up a 2D Camera
            camera = new Camera2D(spriteBatch);

            // Start in the middle of the level
            camera.Position = playerSprite.Position;
            camera.worldWidth = CurrentLevel.WorldWidth;
            camera.worldHeight = CurrentLevel.WorldHeight;
            camera.HasMoved = true;
            visibilityChanged = true;

            // Set up the game HUD
            gameHUD = new HUD(spriteBatch);
            gameHUD.LevelTimer.SetTimer(CurrentLevel.FinishTime);
            gameHUD.LevelTimer.Start();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            playerSprite.Move(gameTime);
            // Are we moving yet?
            if (!butterflySprite.currentMovement.IsMoving())
                butterflySprite.NextMove();
            butterflySprite.Move(gameTime);

            if (playerSprite.CollidesWith(butterflySprite) || playerSprite.CollidesWith(fireflySprite))
                playerIsDead = true;

            List<Tile> tileToCheck = CurrentLevel.GetTiles(playerSprite.bounds);
            foreach (Tile tile in tileToCheck)
            {
                if (tile != null && playerSprite.CollidesWith(tile))
                {
                    collisionDebugString = "Player collides with tile " + tile.Position + " " + tile.TileType;
                    tile.Collision = true;
                }
            }

            //tileToCheck = CurrentLevel.GetTiles(butterflySprite.bounds);
            //foreach (Tile tile in tileToCheck)
            //{
            //    if (tile != null && butterflySprite.CollidesWith(tile))
            //        collisionDebugString = "Butterfly collides with tile " + tile.Position + " " + tile.TileType;
            //}

            // Make camera follow player
            int movement = (int) (playerSprite.Speed*(float) gameTime.ElapsedGameTime.TotalMilliseconds/1000);
            camera.Speed = movement;
            camera.CenterAt(playerSprite.CenterPosition);

            // Update the HUD
            gameHUD.Update(Score.ToString(), LevelTime.ToShortTimeString(), "0");

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Since we're drawing in order from back to front, depth buffer is disabled
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = false;
            graphics.GraphicsDevice.Clear(Color.Black);

            // Prepare scene
            sceneGraph.NewScene();

            CurrentLevel.ResetCollisionDebugInfo();

            // Draw the background
            CurrentLevel.Draw(sceneGraph, spriteBatch, 0, Color.Black);
            // Draw the player
            playerSprite.Draw(sceneGraph, spriteBatch);
            // Draw the firefly
            fireflySprite.Draw(sceneGraph, spriteBatch);
            // Draw the butterfly
            butterflySprite.Draw(sceneGraph, spriteBatch);

            // Write debug info
            sceneGraph.AddText("Player position: " + playerSprite.Position + " Player destination: " + playerSprite.Destination);
            sceneGraph.AddText("Butterfly position: " + butterflySprite.Position + " Butterfly destination: " + butterflySprite.Destination);
            sceneGraph.AddText("Cam position:" + camera.Position);
            sceneGraph.AddText("Tile position:" + CurrentLevel.ToTileCoordinate(playerSprite.Position));
            sceneGraph.AddText("Player is dead:" + playerIsDead);
            sceneGraph.AddText(collisionDebugString);
            sceneGraph.AddText("Player keypress: " + playerSprite.currentMovement.XDirection + " " +
                               playerSprite.currentMovement.YDirection);

            sceneGraph.Draw();

            //playerSprite.MoveStandStill();

            gameHUD.Draw(font, HUDPosition);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles the input from the player so it is possible to move the player sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void HandleInput(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Move player
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.X -= CurrentLevel.CellWidth;
                    playerSprite.MoveLeft();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.X += CurrentLevel.CellWidth;
                    playerSprite.MoveRight();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.Y -= CurrentLevel.CellHeight;
                    playerSprite.MoveUp();
                    return;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (playerSprite.currentMovement.IsMoving() == false)
                {
                    playerSprite.Destination = playerSprite.Position;
                    playerSprite.Destination.Y += CurrentLevel.CellHeight;
                    playerSprite.MoveDown();
                    return;
                }
            }
        }

        /// <summary>
        /// Sets the correct screen size
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event details</param>
        private void OnGraphicsComponentDeviceReset(object sender, EventArgs e)
        {
            displaySize.X = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            displaySize.Y = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            visibilityChanged = true;
        }

        /// <summary>
        /// The player has picked up a diamond
        /// </summary>
        public void DiamondCollected()
        {
            score += 10;
        }
    }
}